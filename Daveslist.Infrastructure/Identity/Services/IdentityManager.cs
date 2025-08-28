using AutoMapper;
using Daveslist.Domain.Shared.Exceptions;
using Daveslist.Domain.Users.Infrastructures;
using Daveslist.Domain.Users.Models;
using Daveslist.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Daveslist.Infrastructure.Identity.Services;

public class IdentityManager : IIdentityManager
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;

    public IdentityManager(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<UserModel> GetByIdAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            throw new KeyNotFoundException("User not found");

        return _mapper.Map<UserModel>(user);
    }

    public async Task RegisterAsync(RegisterModel registerModel)
    {
        var user = _mapper.Map<User>(registerModel);

        var result = await _userManager.CreateAsync(user, registerModel.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BusinessException($"Creating user faced an error: {errors}");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
    }

    public async Task<bool> LoginAsync(LoginModel loginModel)
    {
        var result = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, loginModel.RememberMe, false);

        return result.Succeeded;
    }

    public AuthenticationPropertiesModel GetAuthenticationProperties(string provider, string? redirectUrl)
    {
        var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        return _mapper.Map<AuthenticationPropertiesModel>(authenticationProperties);
    }

    public async Task ExternalLoginCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        if (remoteError != null)
            throw new BusinessException($"Error from external provider: {remoteError}");

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
            throw new BusinessException("Error loading external login information.");

        var email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!,
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!,
                CreatedDate = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                throw new BusinessException("Creating user faced an error.");

            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
                throw new BusinessException("Logging in faced an error.");
        }

        await _signInManager.SignInAsync(user, false);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<string> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            throw new BusinessException("User not found.");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return token;
    }

    public async Task ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            throw new BusinessException("User not found.");

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BusinessException($"Password reset failed: {errors}");
        }
    }

    public async Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<UserModel>>(users);
    }

    public async Task UpdateUserAsync(UserModel userModel)
    {
        var user = _mapper.Map<User>(userModel);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new BusinessException("Updating user faced an error");
    }
}
