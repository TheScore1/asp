namespace Core.HttpLogic.Types;

public record HttpResponse<TResponse> : BaseHttpResponse
{
	/// <summary>
	/// Десериализованное тело ответа, в случае успеха
	/// </summary>
	public required TResponse? Body { get; init; }
	
	public required HttpContent Content { get; init; }
}