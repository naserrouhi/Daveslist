using Daveslist.Application.Users.Dtos;
using Daveslist.Domain.Users.Models;

namespace Daveslist.Application.Users.AppServices.Interfaces;

public interface IUserAppService
{
    AuthenticationPropertiesModel GetAuthenticationProperties(string provider, string? redirectUrl);
    Task LoginAsync(LoginDto loginDto);
    Task LogoutAsync();
    Task RegisterAsync(RegisterDto registerDto);
}
