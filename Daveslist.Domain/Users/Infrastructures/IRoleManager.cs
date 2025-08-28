namespace Daveslist.Domain.Users.Infrastructures;

public interface IRoleManager
{
    Task AssignRoleToUserAsync(string username, string role);
    Task<IEnumerable<string>?> GetUserRolesAsync(string username);
}
