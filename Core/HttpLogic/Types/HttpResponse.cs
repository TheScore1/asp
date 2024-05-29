namespace Core.HttpLogic.Types;

public record HttpResponse<TResponse> : BaseHttpResponse
{
	public required TResponse? Body { get; init; }
	
	public required HttpContent Content { get; init; }
}