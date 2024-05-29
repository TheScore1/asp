using CSharpFunctionalExtensions;
using Logic.Managers.Profile.Dto;
using ProfileConnection.Dto.GetProfiles;

namespace Logic.Managers.Profile;

public interface IProfileManager
{
	Task<Result> Register(RegisterBody body);
	
	Task<Result> DeleteProfile();
	
	Task<Result<GetProfileResponse>> GetProfile();

	Task<Result<GetProfileByIdResponse>> GetProfileById(Guid id);

	Task<Result> UpdateProfile(UpdateProfileBody body);

	Task<Result<GetProfilesByIdResponse>> GetProfiles(GetProfilesByIdRequest request);
}