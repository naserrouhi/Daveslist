using Daveslist.Domain.Shared.Exceptions;
using Daveslist.Domain.Users.Infrastructures;
using Daveslist.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Daveslist.Infrastructure.Identity.Services;

public class RoleManager : IRoleManager
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<UserRole> _roleManager;

    public RoleManager(UserManager<User> userManager, RoleManager<UserRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task AssignRoleToUserAsync(string username, string role)
    {
        var roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
            throw new BusinessException("Role not found.");

        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
            throw new BusinessException("User not found.");

        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new BusinessException($"Failed to assign role. Errors: {errorMessages}");
        }
    }

    public async Task<IEnumerable<string>?> GetUserRolesAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return null;

        return await _userManager.GetRolesAsync(user);
    }
}
