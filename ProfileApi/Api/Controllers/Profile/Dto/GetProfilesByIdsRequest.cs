using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Profile.Dto;

public record GetProfilesByIdsRequest([Required] IEnumerable<Guid> Ids);