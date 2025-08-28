using Daveslist.Application.Categories.AppServices;
using Daveslist.Application.Categories.Mappers;
using Daveslist.Application.Listings.AppServices;
using Daveslist.Application.PrivateMessages.AppServices;
using Daveslist.Application.Users.AppServices;
using Daveslist.Application.Users.AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Daveslist.Application.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserAccountAppService, UserAccountAppService>();
        services.AddScoped<IRoleAppService, RoleAppService>();
        services.AddScoped<ICategoryAppService, CategoryAppService>();
        services.AddScoped<IListingAppService, ListingAppService>();
        services.AddScoped<IPrivateMessageAppService, PrivateMessageAppService>();

        return services;
    }

    public static IServiceCollection AddApplicationMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CategoryMapperProfile));

        return services;
    }
}
