using Daveslist.Domain.Categories.DomainEvents;
using Daveslist.Domain.Categories.DomainServices;
using Daveslist.Domain.Shared.DomainEvents;
using Daveslist.Domain.Shared.Interfaces.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace Daveslist.Domain.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryDomainService, CategoryDomainService>();

        return services;
    }

    public static IServiceCollection AddDomainEvents(this IServiceCollection services)
    {
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddScoped<IDomainEventHandler<CategoryDeletedEvent>, CategoryDeletedEventHandler>();

        return services;
    }
}
