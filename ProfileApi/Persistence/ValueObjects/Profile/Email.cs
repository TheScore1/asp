using Core.Extensions;
using Core.Helpers;
using CSharpFunctionalExtensions;

namespace Persistence.ValueObjects.Profile;

public record Email
{
	public const int EMAIL_MIN_LENGTH = 5;
	public const int EMAIL_MAX_LENGTH = 256;
	
	private Email(string value)
	{
		this.Value = value;
	}
	
	public string Value { get; init; } = null!;

	public static Result<Email> Create(string value)
	{
		if (!ValidationHelper.IsValidEmail(value))
			return Result.Failure<Email>("Невалидный email");
		
		if (string.IsNullOrWhiteSpace(value) || !value.IsInRange(EMAIL_MIN_LENGTH, EMAIL_MAX_LENGTH))
			return Result.Failure<Email>($"Длина почты должна быть от {EMAIL_MIN_LENGTH} до {EMAIL_MAX_LENGTH}");

		return new Email(value);
	}
}