using Daveslist.Domain.Shared.Interfaces.DomainEvents;

namespace Daveslist.Domain.Categories.DomainEvents;

public class CategoryDeletedEvent : IDomainEvent
{
    public CategoryDeletedEvent(int categoryId)
    {
        CategoryId = categoryId;
    }

    public int CategoryId { get; }
}
