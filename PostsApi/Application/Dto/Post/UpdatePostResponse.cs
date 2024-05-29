namespace Application.Dto.Post;

public record UpdatePostResponse(Guid PostId, string Title, string Text, DateTime DateCreated, Guid UserId);