using Daveslist.Domain.Shared.Constants;
using Daveslist.Domain.Shared.Exceptions;
using Daveslist.Domain.Shared.Models;

namespace Daveslist.Domain.Categories.Models;

public class Category : AggregateRoot<int>
{
    protected Category() { }

    public Category(string name, bool isPublic)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > DomainRulesConstants.Category.MaxNameLength)
            throw new BusinessException($"Category name must be 1-{DomainRulesConstants.Category.MaxNameLength} characters.");

        Name = name;
        IsPublic = isPublic;
    }

    public string Name { get; private set; }
    public bool IsPublic { get; private set; }
}
