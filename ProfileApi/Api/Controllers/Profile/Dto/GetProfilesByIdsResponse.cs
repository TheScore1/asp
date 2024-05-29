namespace Api.Controllers.Profile.Dto;

public record GetProfilesByIdsResponse(IEnumerable<ProfileByIdsDto> Profiles);
public record ProfileByIdsDto(Guid Id, string Name, string Surname);