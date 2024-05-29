namespace Core.HttpLogic.Types;

public record HttpRequestData
{
	/// <summary>
	/// Тип метода
	/// </summary>
	public required HttpMethod Method { get; init; }

	/// <summary>
	/// Адрес запроса
	/// </summary>\
	public required Uri Uri { get; init; }

	/// <summary>
	/// Тело метода
	/// </summary>
	public required object Body { get; init; }

	/// <summary>
	/// content-type, указываемый при запросе
	/// </summary>
	public ContentType ContentType { get; init; } = ContentType.ApplicationJson;

	/// <summary>
	/// Заголовки, передаваемые в запросе
	/// </summary>
	// ReSharper disable once CollectionNeverUpdated.Global
	public IDictionary<string, string> HeaderDictionary { get; init; } = new Dictionary<string, string>();

	/// <summary>
	/// Коллекция параметров запроса
	/// </summary>
	// ReSharper disable once CollectionNeverUpdated.Global
	public ICollection<KeyValuePair<string, string>> QueryParameterList { get; init; } =
		new List<KeyValuePair<string, string>>();
}