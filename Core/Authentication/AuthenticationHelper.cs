using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace Core.Authentication;

public class AuthenticationHelper(IHttpContextAccessor httpContextAccessor) : IAuthenticationHelper
{
    public Result<Guid> GetUserId()
    {
        if (httpContextAccessor.HttpContext is null)
            return Result.Failure<Guid>("Cant read http context");

        var id = httpContextAccessor.HttpContext.Request.Headers["UserId"];
        if (id.Count == 0)
            return Result.Failure<Guid>("No header with id");

        return Guid.TryParse(id[0], out var guid)
            ? guid
            : Result.Failure<Guid>("Cant transform to Guid");
    }
}