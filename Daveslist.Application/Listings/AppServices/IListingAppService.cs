using Daveslist.Application.Listings.Models;

namespace Daveslist.Application.Listings.AppServices;

public interface IListingAppService
{
    Task<ListingDto> CreateAsync(UpsertListingDto upsertListingDto, CancellationToken cancellationToken);
    Task DeleteAsync(int id, bool isAdmin, CancellationToken cancellationToken);
    Task<ListingDto> GetAsync(bool isUserAuthenticated, int id, CancellationToken cancellationToken);
    Task<IEnumerable<ListingDto>> GetListAsync(bool isUserAuthenticated, int categoryId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task HideAsync(int id, CancellationToken cancellationToken);
    Task<ListingDto> ReplyAsync(int id, string content, CancellationToken cancellationToken);
    Task UnhideAsync(int id, CancellationToken cancellationToken);
    Task<ListingDto> UpdateAsync(int id, UpsertListingDto upsertListingDto, bool isAdmin, CancellationToken cancellationToken);
}
