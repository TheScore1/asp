namespace Core.HttpLogic.Types;

public record struct HttpConnectionData()
{
	public TimeSpan? Timeout { get; init; } = null;
    
	public CancellationToken CancellationToken { get; init; } = default;
    
	public string? ClientName { get; init; }
}