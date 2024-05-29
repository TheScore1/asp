namespace Application.Dto.Post;

public record UpdatePostBody(Guid PostId, string Title, string Text);