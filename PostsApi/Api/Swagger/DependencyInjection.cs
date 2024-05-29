using System.Reflection;
using Core.Swagger;

namespace API.Swagger;

public static class DependencyInjection
{
	public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
			{
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
		
				c.IncludeXmlComments(xmlPath);
				
				c.OperationFilter<UserIdOperationFilter>();
			}
		);

		return services;
	}
}