using Daveslist.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Daveslist.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void SeedRoles(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();

        string[] roles = [UserRoles.Admin, UserRoles.User, UserRoles.Moderator];

        foreach (var role in roles)
        {
            var isRoleExists = roleManager.RoleExistsAsync(role).GetAwaiter().GetResult();

            if (!isRoleExists)
                roleManager.CreateAsync(new UserRole(role)).GetAwaiter().GetResult();
        }
    }
}
