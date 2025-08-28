using Daveslist.Domain.Categories.Models;
using Daveslist.Domain.Shared.Interfaces.Repositories;

namespace Daveslist.Domain.Categories.Repositories;

public interface ICategoryRepository : IBaseRepository<Category, int>
{
}
