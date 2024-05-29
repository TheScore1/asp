using Application.Dto.Post;
using Mapster;
using Persistence.Entities;

namespace Application.Mapping;

public class PostMappingConfiguration: IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Post, PostDto>()
			.Map(dest => dest.PostId, src => src.Id)
			.Map(dest => dest.Title, src => src.PostInfo.Title)
			.Map(dest => dest.Text, src => src.PostInfo.Text);

		config.NewConfig<Post, CreatePostResponse>()
			.Map(dest => dest.PostId, src => src.Id)
			.Map(dest => dest.Title, src => src.PostInfo.Title)
			.Map(dest => dest.Text, src => src.PostInfo.Text);

		config.NewConfig<Post, UpdatePostResponse>()
			.Map(dest => dest.PostId, src => src.Id)
			.Map(dest => dest.Title, src => src.PostInfo.Title)
			.Map(dest => dest.Text, src => src.PostInfo.Text);
	}
}