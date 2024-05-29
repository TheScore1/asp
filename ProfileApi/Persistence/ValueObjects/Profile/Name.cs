using Core.Extensions;
using CSharpFunctionalExtensions;

namespace Persistence.ValueObjects.Profile;

public record Name
{
	public const int NAME_MIN_LENGTH = 1;
	public const int NAME_MAX_LENGTH = 50;

	private Name(string value)
	{
		this.Value = value;
	}
	
	public string Value { get; init; }

	public static Result<Name> Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name) || !name.IsInRange(NAME_MIN_LENGTH, NAME_MAX_LENGTH))
			return Result.Failure<Name>($"Имя должно иметь длину от {NAME_MIN_LENGTH} до {NAME_MAX_LENGTH}");

		return new Name(name);
	}
}