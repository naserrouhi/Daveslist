using Daveslist.Application.Users.Dtos;

namespace Daveslist.Application.Users.AppServices.Interfaces;

public interface IRoleAppService
{
    Task AssignRoleToUserAsync(AssignRoleDto assignRoleDto);
    Task<IEnumerable<string>?> GetUserRolesAsync(string username);
}
