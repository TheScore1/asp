namespace Application.Dto.Post;

public record GetAllPostsResponse(IEnumerable<PostDto> Posts);

public record PostDto(Guid PostId, string Title, string Text, DateTime DateCreated, Guid UserId);
