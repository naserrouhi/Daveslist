using Daveslist.Domain.Shared.Interfaces.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace Daveslist.Domain.Shared.DomainEvents;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var eventType = domainEvent.GetType();
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

        var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod("Handle");

        if (method is not null)
            await (Task)method.Invoke(handler, [domainEvent, cancellationToken])!;
    }
}
