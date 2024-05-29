using System.Collections.Immutable;
using Application.Dto.Post;
using CSharpFunctionalExtensions;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Factory.Post;
using Persistence.ValueObjects;

namespace Application.Managers.Post;

internal sealed class PostManager(ApplicationDbContext dbContext, IMapper mapper, IPostFactory postFactory): IPostManager
{
	public async Task<Result<GetAllPostsResponse>> GetAllPostsAsync(int page, int countPerPage, CancellationToken cancellationToken = default)
	{
		var posts = await dbContext.Posts.OrderBy(p => p.DateCreated)
			.Skip(countPerPage * (page - 1))
			.Take(countPerPage)
			.ProjectToType<PostDto>()
			.ToListAsync(cancellationToken);

		return new GetAllPostsResponse(posts);
	}

	public async Task<Result<GetPostByIdResponse>> GetPostByIdAsync(Guid postId, CancellationToken cancellationToken = default)
	{
		var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);

		return post is null
			? Result.Failure<GetPostByIdResponse>("Cant find post with this id")
			: mapper.Map<GetPostByIdResponse>(post);
	}

	public async Task<Result<CreatePostResponse>> CreatePost(CreatePostBody body, CancellationToken cancellationToken = default)
	{
		var createNewPostResult = postFactory.CreatePost(body.UserId, body.Title, body.Text);

		if (createNewPostResult.IsFailure)
			return Result.Failure<CreatePostResponse>(createNewPostResult.Error);

		var addNewPostResult = await dbContext.Posts.AddAsync(createNewPostResult.Value, cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);

		return mapper.Map<CreatePostResponse>(addNewPostResult.Entity);
	}

	public async Task<Result<UpdatePostResponse>> UpdatePost(UpdatePostBody body, CancellationToken cancellationToken = default)
	{
		var postWithThisId = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == body.PostId, cancellationToken);
		if (postWithThisId is null)
			return Result.Failure<UpdatePostResponse>("Cant find post with this id");
		
		var (_, isCreatePostInfoFailure, newPostInfo, newPostInfoError) = PostInfo.Create(body.Title, body.Text);
		if (isCreatePostInfoFailure)
			return Result.Failure<UpdatePostResponse>(newPostInfoError);

		postWithThisId.PostInfo = newPostInfo;
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return Result.Success(mapper.Map<UpdatePostResponse>(postWithThisId));
	}

	public async Task<Result> DeletePost(Guid postId, CancellationToken cancellationToken = default)
	{
		var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
		if (post is null)
			return Result.Failure("Cant find post with this id");

		dbContext.Posts.Remove(post);
		await dbContext.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}