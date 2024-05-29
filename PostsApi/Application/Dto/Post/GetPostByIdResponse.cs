namespace Application.Dto.Post;

public record GetPostByIdResponse(Guid PostId, string Title, string Text, DateTime DateCreated, Guid UserId);