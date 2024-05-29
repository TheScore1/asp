using CSharpFunctionalExtensions;

namespace Core.Authentication;

public interface IAuthenticationHelper
{
    Result<Guid> GetUserId();
}