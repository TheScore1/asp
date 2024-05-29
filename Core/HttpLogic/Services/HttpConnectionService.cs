using Core.HttpLogic.Services.Interfaces;
using Core.HttpLogic.Types;

namespace Core.HttpLogic.Services;

/// <inheritdoc />
internal class HttpConnectionService : IHttpConnectionService
{
	private readonly IHttpClientFactory _httpClientFactory;

	///
	public HttpConnectionService(IHttpClientFactory httpClientFactory)
	{
		this._httpClientFactory = httpClientFactory;
	}

	/// <inheritdoc />
	public HttpClient CreateHttpClient(HttpConnectionData httpConnectionData)
	{
		var httpClient = string.IsNullOrWhiteSpace(httpConnectionData.ClientName)
			? this._httpClientFactory.CreateClient()
			: this._httpClientFactory.CreateClient(httpConnectionData.ClientName);

		if (httpConnectionData.Timeout != null)
			httpClient.Timeout = httpConnectionData.Timeout.Value;

		return httpClient;
	}

	/// <inheritdoc />
	public Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage httpRequestMessage, HttpClient httpClient,
		CancellationToken cancellationToken,
		HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
	{
		return httpClient.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken);
	}
}