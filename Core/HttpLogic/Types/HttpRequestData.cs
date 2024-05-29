namespace Core.HttpLogic.Types;

public record HttpRequestData
{
	public required HttpMethod Method { get; init; }

	public required Uri Uri { get; init; }

	public required object Body { get; init; }

	public ContentType ContentType { get; init; } = ContentType.ApplicationJson;

	public IDictionary<string, string> HeaderDictionary { get; init; } = new Dictionary<string, string>();

	public ICollection<KeyValuePair<string, string>> QueryParameterList { get; init; } =
		new List<KeyValuePair<string, string>>();
}