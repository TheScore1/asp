namespace Api.Controllers.Profile.Dto;

public record GetProfileResponse(Guid Id, string Email, string Name, string Surname);