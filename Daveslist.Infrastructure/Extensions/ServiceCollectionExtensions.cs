using Daveslist.Domain.Categories.Repositories;
using Daveslist.Domain.Listings.Repositories;
using Daveslist.Domain.PrivateMessages.Repositories;
using Daveslist.Domain.Users.Infrastructures;
using Daveslist.Infrastructure.Data;
using Daveslist.Infrastructure.Data.Repositories;
using Daveslist.Infrastructure.ExceptionHandlers;
using Daveslist.Infrastructure.Identity.Mappers;
using Daveslist.Infrastructure.Identity.Models;
using Daveslist.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Daveslist.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        })
        .AddCookie(IdentityConstants.ApplicationScheme, options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.Name = "Daveslist.Auth";
            options.LoginPath = "/login";
            options.AccessDeniedPath = "/access-denied";
            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                },
                OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();

        services.AddIdentityCore<User>()
                .AddRoles<UserRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<DaveslistDbContext>()
                .AddApiEndpoints();

        services.AddHttpContextAccessor();

        services.AddScoped<IIdentityManager, IdentityManager>();
        services.AddScoped<IRoleManager, RoleManager>();
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DaveslistDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddInfrastructureMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(IdentityMapperProfile));

        return services;
    }

    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();
        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}
