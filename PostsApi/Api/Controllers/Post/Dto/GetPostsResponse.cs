using System.Linq.Expressions;

namespace Api.Controllers.Post.Dto;

public record GetPostsResponse(List<PostDto> Posts, bool IsNextPageExists);