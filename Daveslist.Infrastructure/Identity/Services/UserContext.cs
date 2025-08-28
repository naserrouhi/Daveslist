using Daveslist.Domain.Users.Infrastructures;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Daveslist.Infrastructure.Identity.Services;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userIdClaimValue = user?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaimValue is null)
            return null;

        return int.Parse(userIdClaimValue);
    }
}
