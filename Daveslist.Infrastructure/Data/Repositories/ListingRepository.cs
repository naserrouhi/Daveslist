using Daveslist.Domain.Listings.Models;
using Daveslist.Domain.Listings.Repositories;

namespace Daveslist.Infrastructure.Data.Repositories;

public class ListingRepository : BaseRepository<Listing, int>, IListingRepository
{
    public ListingRepository(DaveslistDbContext dbContext) : base(dbContext)
    {
    }
}
