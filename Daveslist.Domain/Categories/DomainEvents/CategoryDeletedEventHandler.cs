using Daveslist.Domain.Listings.Repositories;
using Daveslist.Domain.Shared.Interfaces.DomainEvents;

namespace Daveslist.Domain.Categories.DomainEvents;

public class CategoryDeletedEventHandler : IDomainEventHandler<CategoryDeletedEvent>
{
    private readonly IListingRepository _listingRepository;

    public CategoryDeletedEventHandler(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task Handle(CategoryDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        var listings = await _listingRepository.GetListAsync(l => l.CategoryId == domainEvent.CategoryId, cancellationToken);

        listings.ToList().ForEach(l => l.MoveToTrash());
    }
}
