using Application.Dto.Post;
using CSharpFunctionalExtensions;

namespace Application.Managers.Post;

public interface IPostManager
{
	Task<Result<GetAllPostsResponse>> GetAllPostsAsync(int page, int countPerPage, CancellationToken cancellationToken = default);
	
	Task<Result<GetPostByIdResponse>> GetPostByIdAsync(Guid postId, CancellationToken cancellationToken = default);

	Task<Result<CreatePostResponse>> CreatePost(CreatePostBody body, CancellationToken cancellationToken = default);

	Task<Result<UpdatePostResponse>> UpdatePost(UpdatePostBody body, CancellationToken cancellationToken = default);

	Task<Result> DeletePost(Guid postId, CancellationToken cancellationToken = default);
}

