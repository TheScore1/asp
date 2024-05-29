namespace ProfileConnection.Dto.GetProfiles;

public record GetProfilesByIdRequest(IEnumerable<Guid> Ids);