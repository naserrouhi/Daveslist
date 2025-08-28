using Daveslist.Domain.Categories.Models;

namespace Daveslist.Domain.Categories.DomainServices;

public interface ICategoryDomainService
{
    Task<Category> CreateAsync(string name, bool isPublic, CancellationToken cancellationToken);
}
