using Api.Controllers.Profile.Dto;
using Core.Dto;
using Logic.Managers.Profile;
using Logic.Managers.Profile.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using ProfileConnection.Dto.GetProfiles;
using GetProfileByIdResponse = Api.Controllers.Profile.Dto.GetProfileByIdResponse;
using GetProfileResponse = Api.Controllers.Profile.Dto.GetProfileResponse;

namespace Api.Controllers.Profile;

[ApiController]
[Route("api/profile")]
public class ProfileController(IProfileManager profileManager, IMapper mapper): ControllerBase
{
	/// <summary>
	/// Регистрация пользователя
	/// </summary>
	/// <param name="request">Запрос</param>
	/// <returns>Результат</returns>
	[HttpPost("register")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Register([FromBody] RegisterRequest request)
	{
		var result = await profileManager.Register(new RegisterBody(request.Email, request.Name, request.Surname));

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: NoContent();
	}
	
	/// <summary>
	/// Получить свой профиль
	/// </summary>
	/// <returns>Результат получения</returns>
	[HttpGet]
	[ProducesResponseType<GetProfileResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetProfile()
	{
		var result = await profileManager.GetProfile();

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: Ok(mapper.Map<GetProfileResponse>(result.Value));
	}

	/// <summary>
	/// Получить профиль по id
	/// </summary>
	/// <param name="id">Id профиля</param>
	/// <returns>Результат обновления</returns>
	[HttpGet("{id:guid}")]
	[ProducesResponseType<GetProfileByIdResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetProfileById([FromRoute] Guid id)
	{
		var result = await profileManager.GetProfileById(id);

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: Ok(mapper.Map<GetProfileByIdResponse>(result.Value));
	}

	/// <summary>
	/// Обновить свой профиль
	/// </summary>
	/// <returns>Результат обновления</returns>
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
	{
		var result = await profileManager.UpdateProfile(mapper.Map<UpdateProfileBody>(request));

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: NoContent();
	}

	/// <summary>
	/// Удалить профиль
	/// </summary>
	/// <returns>Результат удаления</returns>
	[HttpDelete("delete")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> DeleteProfile()
	{
		var result = await profileManager.DeleteProfile();

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: NoContent();
	}

	[HttpPost("/byIds")]
	[ProducesResponseType<GetProfilesByIdsResponse>(StatusCodes.Status200OK)]
	public async Task<IActionResult> GetProfilesById([FromBody] GetProfilesByIdRequest request)
	{
		var result = await profileManager.GetProfiles(request);

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: Ok(result.Value);
	}
}