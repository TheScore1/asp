namespace ProfileConnection.Dto.GetProfiles;

public record GetProfilesByIdResponse
{
	public required IEnumerable<GetProfilesDto> Profiles { get; init; }
};
public record GetProfilesDto(Guid Id, string Name, string Surname);