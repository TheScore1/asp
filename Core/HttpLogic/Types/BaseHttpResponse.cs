using System.Net;
using System.Net.Http.Headers;

namespace Core.HttpLogic.Types;

public record BaseHttpResponse
{
	/// <summary>
	/// Статус ответа
	/// </summary>
	public required HttpStatusCode StatusCode { get; init; }

	/// <summary>
	/// Заголовки, передаваемые в ответе
	/// </summary>
	public required HttpResponseHeaders Headers { get; init; }

	/// <summary>
	/// Заголовки контента
	/// </summary>
	public required HttpContentHeaders ContentHeaders { get; init; }

	/// <summary>
	/// Является ли статус код успешным
	/// </summary>
	public bool IsSuccessStatusCode
	{
		get
		{
			var statusCode = (int)this.StatusCode;

			return statusCode is >= 200 and <= 299;
		}
	}
}