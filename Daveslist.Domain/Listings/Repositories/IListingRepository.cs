using Daveslist.Domain.Listings.Models;
using Daveslist.Domain.Shared.Interfaces.Repositories;

namespace Daveslist.Domain.Listings.Repositories;

public interface IListingRepository : IBaseRepository<Listing, int>
{
}
