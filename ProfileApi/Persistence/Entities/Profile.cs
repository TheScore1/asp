using System.Diagnostics.CodeAnalysis;
using Core.DbEntity;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Persistence.ValueObjects;
using Persistence.ValueObjects.Profile;

namespace Persistence.Entities;

public class Profile: IDbEntity<Guid>
{
	public const int EMAIL_MIN_LENGTH = 5;
	public const int EMAIL_MAX_LENGTH = 256;
	
	private Profile(Guid id, Email email, Name name, Surname surname)
	{
		this.Id = id;
		this.Email = email;
		this.Name = name;
		this.Surname = surname;
	}
	
	public Guid Id { get; init; }
	public Email Email { get; private set; }
	public Name Name { get; private set; }
	public Surname Surname { get; private set; }

	public static Result<Profile> Create(Guid id, string email, string name, string surname)
	{
		var (_, emailResultFailure, emailResult, emailError) = Email.Create(email);
		if (emailResultFailure)
			return Result.Failure<Profile>(emailError);

		var (_, nameResultFailure, nameResult, nameError) = Name.Create(name);
		if (nameResultFailure)
			return Result.Failure<Profile>(nameError);

		var (_, surnameResultFailure, surnameResult, surnameError) = Surname.Create(surname);
		if (surnameResultFailure)
			return Result.Failure<Profile>(surnameError);

		return new Profile(id, emailResult, nameResult, surnameResult);
	}

	public Result UpdateProfile(string name, string surname)
	{
		var newNameResult = Name.Create(name);
		if (newNameResult.IsFailure)
			return Result.Failure(newNameResult.Error);

		var newSurnameResult = Surname.Create(surname);
		if (newSurnameResult.IsFailure)
			return Result.Failure(newSurnameResult.Error);

		this.Name = newNameResult.Value;
		this.Surname = newSurnameResult.Value;

		return Result.Success();
	}
}