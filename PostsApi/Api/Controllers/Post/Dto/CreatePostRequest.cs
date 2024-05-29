using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Post.Dto;

public record CreatePostRequest(
	[Required] string Title,
	[Required] string Text);