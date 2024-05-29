using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Factory.Post;

namespace Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseNpgsql(configuration.GetConnectionString("Db"));
		});

		services.AddSingleton<IPostFactory, PostFactory>();
		
		return services;
	}
}