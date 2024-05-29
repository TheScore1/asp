using System.Net;
using System.Net.Http.Headers;

namespace Core.HttpLogic.Types;

public record BaseHttpResponse
{
	public required HttpStatusCode StatusCode { get; init; }

	public required HttpResponseHeaders Headers { get; init; }

	public required HttpContentHeaders ContentHeaders { get; init; }

	public bool IsSuccessStatusCode
	{
		get
		{
			var statusCode = (int)this.StatusCode;

			return statusCode is >= 200 and <= 299;
		}
	}
}