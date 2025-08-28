using AutoMapper;
using Daveslist.Domain.Shared.Exceptions;
using Daveslist.Domain.Users.Infrastructures;
using Daveslist.Domain.Users.Models;
using Daveslist.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;

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

    public async Task<UserModel?> FindByIdAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return _mapper.Map<UserModel?>(user);
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

        var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.User);
        if (!roleResult.Succeeded)
        {
            var errorMessages = string.Join("; ", roleResult.Errors.Select(e => e.Description));
            throw new BusinessException($"Failed to assign role. Errors: {errorMessages}");
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

    public async Task LogoutAsync() => await _signInManager.SignOutAsync();
}
