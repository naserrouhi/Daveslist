using Daveslist.Application.Categories.Models;

namespace Daveslist.Application.Categories.AppServices;

public interface ICategoryAppService
{
    Task<CategoryDto> CreateAsync(string name, bool isPublic, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<CategoryDto>> GetListAsync(CancellationToken cancellationToken);
}
