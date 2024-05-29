using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Profile.Dto;

public record UpdateProfileRequest([Required] string Name, [Required] string Surname);