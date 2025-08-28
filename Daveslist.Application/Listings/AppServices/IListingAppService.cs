using Daveslist.Application.Listings.Models;

namespace Daveslist.Application.Listings.AppServices;

public interface IListingAppService
{
    Task<ListingDto> CreateAsync(UpsertListingDto upsertListingDto, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<ListingDto> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<ListingDto>> GetListAsync(int categoryId, CancellationToken cancellationToken);
    Task<IEnumerable<ReplyDto>> GetRepliesAsync(int id, CancellationToken cancellationToken);
    Task HideAsync(int id, CancellationToken cancellationToken);
    Task<ListingDto> ReplyAsync(int id, string content, CancellationToken cancellationToken);
    Task UnhideAsync(int id, CancellationToken cancellationToken);
    Task<ListingDto> UpdateAsync(int id, UpsertListingDto upsertListingDto, CancellationToken cancellationToken);
}
