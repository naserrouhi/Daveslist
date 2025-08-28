using Daveslist.Application.Users.AppServices.Interfaces;
using Daveslist.Application.Users.Dtos;
using Daveslist.Domain.Users.Infrastructures;

namespace Daveslist.Application.Users.AppServices;

public class RoleAppService : IRoleAppService
{
    private readonly IRoleManager _roleManager;

    public RoleAppService(IRoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task AssignRoleToUserAsync(AssignRoleDto assignRoleDto)
    {
        await _roleManager.AssignRoleToUserAsync(assignRoleDto.Username, assignRoleDto.Role);
    }

    public async Task<IEnumerable<string>?> GetUserRolesAsync(string username)
    {
        return await _roleManager.GetUserRolesAsync(username);
    }
}
