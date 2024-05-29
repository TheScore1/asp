using CSharpFunctionalExtensions;
using Persistence.ValueObjects;
using Persistence.Entities;

namespace Persistence.Factory.Post;

public class PostFactory: IPostFactory
{
	public Result<Entities.Post> CreatePost(Guid userId, string title, string text)
	{
		var (_, isCreatePostInfoFailure, postInfo, postInfoError) = PostInfo.Create(title, text);
		if (isCreatePostInfoFailure)
			return Result.Failure<Entities.Post>(postInfoError);

		var (_, isCreatePostError, post, createPostError) = Entities.Post.Create(userId, postInfo);
		return isCreatePostError
			? Result.Failure<Entities.Post>(createPostError)
			: post;
	}
}