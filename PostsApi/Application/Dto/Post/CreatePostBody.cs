namespace Application.Dto.Post;

public record CreatePostBody(Guid UserId, string Title, string Text);