using Core.HttpLogic.Services.Interfaces;
using Core.HttpLogic.Types;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfileConnection.Dto.GetProfiles;
using ProfileConnection.Interfaces;

namespace ProfileConnection;

public class ProfileConnectionService : IProfileConnectionService
{
	private readonly IHttpRequestService _httpRequestService = null!;
	private readonly ProfileConnectionOptions _profileConnectionOptions;

	public ProfileConnectionService(IConfiguration configuration, IServiceProvider serviceProvider, ProfileConnectionOptions profileConnectionOptions)
	{
		this._profileConnectionOptions = profileConnectionOptions;

		if (profileConnectionOptions.ProfileConnectionType == "http")
			this._httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
		else
		{
			// TODO rpc
		}
	}

	public async Task<Result<GetProfilesByIdResponse>> GetProfiles(GetProfilesByIdRequest request)
	{
		var response = await this._httpRequestService.SendRequestAsync<GetProfilesByIdResponse>(new HttpRequestData()
		{
			Uri = new Uri($"{this._profileConnectionOptions.ProfileConnectionUrl}/api/profile"), // Todo узнать какой url,
			Method = HttpMethod.Get,
			ContentType = ContentType.ApplicationJson,
			Body = request.Ids
		});

		if (!response.IsSuccessStatusCode || response.Body == null)
			return Result.Failure<GetProfilesByIdResponse>("Request for profiles got error");

		return response.Body;
	}
}