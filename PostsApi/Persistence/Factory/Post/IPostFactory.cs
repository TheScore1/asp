using CSharpFunctionalExtensions;

namespace Persistence.Factory.Post;

public interface IPostFactory
{
	Result<Entities.Post> CreatePost(Guid userId, string title, string text);
}