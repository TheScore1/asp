using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Profile.Dto;

public record RegisterRequest(
	[Required] [EmailAddress] string Email,
	[Required] string Name,
	[Required] string Surname);