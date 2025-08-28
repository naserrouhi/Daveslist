using Daveslist.Domain.Users.Models;

namespace Daveslist.Domain.Users.Infrastructures;

public interface IIdentityManager
{
    Task ExternalLoginCallbackAsync(string? returnUrl = null, string? remoteError = null);
    Task<string> ForgotPasswordAsync(string email);
    AuthenticationPropertiesModel GetAuthenticationProperties(string provider, string? redirectUrl);
    Task<bool> LoginAsync(LoginModel loginModel);
    Task LogoutAsync();
    Task RegisterAsync(RegisterModel registerModel);
    Task<UserModel> GetByIdAsync(int userId);
    Task ResetPasswordAsync(string email, string token, string newPassword);
    Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken);
    Task UpdateUserAsync(UserModel userModel);
}
