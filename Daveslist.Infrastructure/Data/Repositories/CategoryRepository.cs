using Daveslist.Domain.Categories.Models;
using Daveslist.Domain.Categories.Repositories;

namespace Daveslist.Infrastructure.Data.Repositories;

public class CategoryRepository : BaseRepository<Category, int>, ICategoryRepository
{
    public CategoryRepository(DaveslistDbContext dbContext) : base(dbContext)
    {
    }
}
