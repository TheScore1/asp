using Core.Extensions;
using CSharpFunctionalExtensions;

namespace Persistence.ValueObjects.Profile;

public record Surname
{
	public const int SURNAME_MIN_LENGTH = 1;
	public const int SURNAME_MAX_LENGTH = 50;

	private Surname(string value)
	{
		this.Value = value;
	}
	
	public string Value { get; init; }

	public static Result<Surname> Create(string surname)
	{
		if (string.IsNullOrWhiteSpace(surname) || !surname.IsInRange(SURNAME_MIN_LENGTH, SURNAME_MAX_LENGTH))
			return Result.Failure<Surname>($"Имя должно иметь длину от {SURNAME_MIN_LENGTH} до {SURNAME_MAX_LENGTH}");

		return new Surname(surname);
	}
}