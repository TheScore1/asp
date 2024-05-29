using Api.Controllers.Post.Dto;
using Application.Managers.Post;
using Application.Managers.Post.Dto;
using Core.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using GetPostByIdResponse = Api.Controllers.Post.Dto.GetPostByIdResponse;
using GetPostsResponse = Api.Controllers.Post.Dto.GetPostsResponse;

namespace Api.Controllers.Post;

[ApiController]
[Route("api/post")]
[Produces("application/json")]
public class PostController(IPostManager postManager, IMapper mapper) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        var result = await postManager.CreatePost(mapper.Map<CreatePostBody>(request));

        return result.IsFailure
            ? BadRequest(new ErrorResponse(result.Error))
            : NoContent();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<GetPostByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPostById([FromRoute] Guid id)
    {
        var result = await postManager.GetPostById(id);

        return result.IsFailure
            ? BadRequest(new ErrorResponse(result.Error))
            : Ok(mapper.Map<GetPostByIdResponse>(result.Value));
    }

    [HttpGet]
    [ProducesResponseType<GetPostsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPosts([FromQuery] int page = 1, [FromQuery] int countPerPage = 10)
    {
        var result = await postManager.GetPosts(page, countPerPage);

        return result.IsFailure
            ? BadRequest(new ErrorResponse(result.Error))
            : Ok(mapper.Map<GetPostsResponse>(result.Value));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePost([FromRoute] Guid id)
    {
        var result = await postManager.DeletePost(id);

        return result.IsFailure
            ? BadRequest(new ErrorResponse(result.Error))
            : NoContent();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<GetPostByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePost([FromRoute] Guid id, [FromBody] UpdatePostRequest request)
    {
        var result = await postManager.UpdatePost(id, mapper.Map<UpdatePostBody>(request));

        return result.IsFailure
            ? BadRequest(new ErrorResponse(result.Error))
            : Ok(mapper.Map<GetPostByIdResponse>(result.Value));
    }
}