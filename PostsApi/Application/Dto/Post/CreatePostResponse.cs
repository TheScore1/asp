namespace Application.Dto.Post;

public record CreatePostResponse(
	Guid PostId,
	string Title,
	string Text,
	DateTime DateCreated,
	Guid UserId);