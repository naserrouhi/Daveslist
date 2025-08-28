using Microsoft.AspNetCore.Identity;

namespace Daveslist.Infrastructure.Identity.Models;

public class UserRole : IdentityRole<int>
{
    public UserRole() : base() { }

    public UserRole(string role) : base(role)
    {
    }
}
