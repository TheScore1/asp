using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using Core.HttpLogic.Services.Interfaces;
using Core.HttpLogic.Types;
using Core.TraceLogic.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ContentType = Core.HttpLogic.Types.ContentType;

namespace Core.HttpLogic.Services;

internal class HttpRequestService : IHttpRequestService
{
	private readonly IHttpConnectionService _httpConnectionService;
	private readonly IEnumerable<ITraceWriter> _traceWriterList;

	public HttpRequestService(
		IHttpConnectionService httpConnectionService,
		IEnumerable<ITraceWriter> traceWriterList)
	{
		this._httpConnectionService = httpConnectionService;
		this._traceWriterList = traceWriterList;
	}

	public async Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData,
		HttpConnectionData connectionData)
	{
		var client = this._httpConnectionService.CreateHttpClient(connectionData);

		var httpRequestMessage = new HttpRequestMessage();

		httpRequestMessage.RequestUri =
			new Uri(QueryHelpers.AddQueryString(requestData.Uri.AbsoluteUri, requestData.QueryParameterList!));
		httpRequestMessage.Method = requestData.Method;
		httpRequestMessage.Content = PrepareContent(requestData.Body, requestData.ContentType);
		foreach (var header in requestData.HeaderDictionary)
			httpRequestMessage.Headers.Add(header.Key, header.Value);

		foreach (var traceWriter in this._traceWriterList)
			httpRequestMessage.Headers.Add(traceWriter.Name, traceWriter.GetValue());

		var res = await this._httpConnectionService.SendRequestAsync(httpRequestMessage, client, default);
		return new HttpResponse<TResponse>
		{
			StatusCode = res.StatusCode,
			Headers = res.Headers,
			ContentHeaders = res.Content.Headers,
			Body = res.IsSuccessStatusCode ? 
				await ParseContent<TResponse>(res) 
				: default,
			Content = res.Content
		};
	}

	private static HttpContent PrepareContent(object body, ContentType contentType)
	{
		switch (contentType)
		{
			case ContentType.ApplicationJson:
			{
				if (body is string stringBody)
				{
					body = JToken.Parse(stringBody);
				}

				var serializeSettings = new JsonSerializerSettings
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					NullValueHandling = NullValueHandling.Ignore
				};

				var serializedBody = JsonConvert.SerializeObject(body, serializeSettings);
				var content = new StringContent(serializedBody, Encoding.UTF8, MediaTypeNames.Application.Json);
				return content;
			}

			case ContentType.XWwwFormUrlEncoded:
			{
				if (body is not IEnumerable<KeyValuePair<string, string>> list)
				{
					throw new Exception(
						$"Body for content type {contentType} must be {typeof(IEnumerable<KeyValuePair<string, string>>).Name}");
				}

				return new FormUrlEncodedContent(list);
			}
			case ContentType.ApplicationXml:
			{
				if (body is not string s)
				{
					throw new Exception($"Body for content type {contentType} must be XML string");
				}

				return new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Xml);
			}
			case ContentType.Binary:
			{
				if (body.GetType() != typeof(byte[]))
				{
					throw new Exception($"Body for content type {contentType} must be {typeof(byte[]).Name}");
				}

				return new ByteArrayContent((byte[])body);
			}
			case ContentType.TextXml:
			{
				if (body is not string s)
				{
					throw new Exception($"Body for content type {contentType} must be XML string");
				}

				return new StringContent(s, Encoding.UTF8, MediaTypeNames.Text.Xml);
			}
			default:
				throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
		}
	}

	private static async Task<TResponse?> ParseContent<TResponse>(HttpResponseMessage response)
	{
		if (response.Content.Headers.ContentType is null)
			throw new ArgumentException("Cant define content type");

		return response.Content.Headers.ContentType.MediaType switch
		{
			MediaTypeNames.Application.Json => await response.Content.ReadFromJsonAsync<TResponse>(),
			_ => throw new NotImplementedException("App doesnt support this content type")
		};
	}
}