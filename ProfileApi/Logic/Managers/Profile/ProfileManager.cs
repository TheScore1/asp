using Core.Authentication;
using CSharpFunctionalExtensions;
using Logic.Managers.Profile.Dto;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using ProfileConnection.Dto.GetProfiles;

namespace Logic.Managers.Profile;

public class ProfileManager(ProfileDbContext dbContext, IAuthenticationHelper authenticationHelper, IMapper mapper) : IProfileManager
{
	public async Task<Result> Register(RegisterBody body)
	{
		var (_, profileFailure, profile, profileError) =
			Persistence.Entities.Profile.Create(Guid.NewGuid(), body.Email, body.Name, body.Surname);
		if (profileFailure)
			return Result.Failure(profileError);

		await dbContext.Profiles.AddAsync(profile);
		await dbContext.SaveChangesAsync();

		return Result.Success();
	}

	public async Task<Result> DeleteProfile()
	{
		var userIdResult = authenticationHelper.GetUserId();
		if (userIdResult.IsFailure)
			return Result.Failure("Cant read user id");

		var user = await dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == userIdResult.Value);
		if (user is null)
			return Result.Failure("Cant find user with this id");

		dbContext.Profiles.Remove(user);
		await dbContext.SaveChangesAsync();

		return Result.Success();

	}

	public async Task<Result<GetProfileResponse>> GetProfile()
	{
		var (_, userIdFailure, userId, userIdError) = authenticationHelper.GetUserId();
		if (userIdFailure)
			return Result.Failure<GetProfileResponse>(userIdError);

		var profile = await dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == userId);
		return profile is null
			? Result.Failure<GetProfileResponse>("No user with this id")
			: mapper.Map<GetProfileResponse>(profile);
	}

	public async Task<Result<GetProfileByIdResponse>> GetProfileById(Guid id)
	{
		var profile = await dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == id);

		return profile is null
			? Result.Failure<GetProfileByIdResponse>("No user with this id")
			: mapper.Map<GetProfileByIdResponse>(profile);
	}

	public async Task<Result> UpdateProfile(UpdateProfileBody body)
	{
		var userIdResult = authenticationHelper.GetUserId();
		if (userIdResult.IsFailure)
			return Result.Failure(userIdResult.Error);

		var profile = await dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == userIdResult.Value);
		if (profile is null)
			return Result.Failure("No user with this id");

		var updateResult = profile.UpdateProfile(body.Name, body.Surname);
		if (updateResult.IsFailure)
			return Result.Failure(updateResult.Error);

		await dbContext.SaveChangesAsync();

		return Result.Success();
	}

	public async Task<Result<GetProfilesByIdResponse>> GetProfiles(GetProfilesByIdRequest request)
	{
		var profiles = await dbContext.Profiles.Where(p => request.Ids.Contains(p.Id))
			.ProjectToType<GetProfilesDto>()
			.ToListAsync();

		return new GetProfilesByIdResponse
		{
			Profiles = profiles
		};
	}
}