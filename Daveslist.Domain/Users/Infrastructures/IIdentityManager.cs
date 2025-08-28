using Daveslist.Domain.Users.Models;

namespace Daveslist.Domain.Users.Infrastructures;

public interface IIdentityManager
{
    AuthenticationPropertiesModel GetAuthenticationProperties(string provider, string? redirectUrl);
    Task<bool> LoginAsync(LoginModel loginModel);
    Task LogoutAsync();
    Task RegisterAsync(RegisterModel registerModel);
    Task<UserModel?> FindByIdAsync(int userId);
}
