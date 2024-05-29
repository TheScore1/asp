using Core.Entity;
using CSharpFunctionalExtensions;
using Persistence.ValueObjects;

namespace Persistence.Entities;

public class Post: IEntity<Guid>
{
	private Post() {}
	
	public required Guid Id { get; set; }
	public required PostInfo PostInfo { get; set; }
	public required DateTime DateCreated { get; set; }
	public required Guid UserId { get; set; }
	

	public static Result<Post> Create(Guid userId, PostInfo postInfo)
	{
		return new Post
		{
			Id = Guid.NewGuid(),
			UserId = userId,
			DateCreated = DateTime.UtcNow,
			PostInfo = postInfo
		};
	}
}

