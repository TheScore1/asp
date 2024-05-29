using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace Core.Authentication;

public class AuthenticationHelper(IHttpContextAccessor httpContextAccessor) : IAuthenticationHelper
{
    public Result<Guid> GetUserId()
    {
        if (httpContextAccessor.HttpContext is null)
            return Result.Failure<Guid>("Не удалось прочитать http context");

        var id = httpContextAccessor.HttpContext.Request.Headers["UserId"];
        if (id.Count == 0)
            return Result.Failure<Guid>("Нет заголовка с id");

        return Guid.TryParse(id[0], out var guid)
            ? guid
            : Result.Failure<Guid>("Не удалось преобразовать к Guid");
    }
}