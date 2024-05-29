using CSharpFunctionalExtensions;
using ProfileConnection.Dto.GetProfiles;

namespace ProfileConnection.Interfaces;

public interface IProfileConnectionService
{
	Task<Result<GetProfilesByIdResponse>> GetProfiles(GetProfilesByIdRequest request);
}