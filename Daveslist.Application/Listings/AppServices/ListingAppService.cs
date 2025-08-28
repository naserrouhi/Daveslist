using AutoMapper;
using Daveslist.Application.Listings.Models;
using Daveslist.Domain.Categories.Repositories;
using Daveslist.Domain.Listings.Models;
using Daveslist.Domain.Listings.Repositories;
using Daveslist.Domain.Users.Infrastructures;

namespace Daveslist.Application.Listings.AppServices;

public class ListingAppService : IListingAppService
{
    private readonly IListingRepository _listingRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public ListingAppService(IListingRepository listingRepository,
                             ICategoryRepository categoryRepository,
                             IUserContext userContext,
                             IMapper mapper)
    {
        _listingRepository = listingRepository;
        _categoryRepository = categoryRepository;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ListingDto>> GetListAsync(int categoryId, CancellationToken cancellationToken)
    {
        var listings = await _listingRepository.GetListAsync(l => l.CategoryId == categoryId, cancellationToken);

        return _mapper.Map<IEnumerable<ListingDto>>(listings);
    }

    public async Task<ListingDto> GetAsync(int id, CancellationToken cancellationToken)
    {
        var listing = await GetListingAsync(id, cancellationToken);

        return _mapper.Map<ListingDto>(listing);
    }

    public async Task<ListingDto> CreateAsync(UpsertListingDto upsertListingDto, CancellationToken cancellationToken)
    {
        var isCategoryValid = await _categoryRepository.AnyAsync(c => c.Id == upsertListingDto.CategoryId, cancellationToken);

        if (!isCategoryValid)
            throw new KeyNotFoundException("Category not found.");

        var userId = _userContext.GetCurrentUserId().GetValueOrDefault();
        var listing = _mapper.Map<Listing>(upsertListingDto, opt => opt.Items["UserId"] = userId);

        await _listingRepository.AddAsync(listing, cancellationToken);
        await _listingRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ListingDto>(listing);
    }

    public async Task<ListingDto> UpdateAsync(int id, UpsertListingDto upsertListingDto, CancellationToken cancellationToken)
    {
        var listing = await GetListingAsync(id, cancellationToken);
        var pictures = upsertListingDto.PictureUrls?.Select(u => new Picture(u)).ToList();

        listing.Update(upsertListingDto.CategoryId,
                              upsertListingDto.Title!,
                              upsertListingDto.Content!,
                              upsertListingDto.IsPublic,
                              pictures);

        await _listingRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ListingDto>(listing);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var listing = await GetListingAsync(id, cancellationToken);

        _listingRepository.Remove(listing);
        await _listingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task HideAsync(int id, CancellationToken cancellationToken)
    {
        var listing = await GetListingAsync(id, cancellationToken);

        listing.Hide();
        await _listingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UnhideAsync(int id, CancellationToken cancellationToken)
    {
        var listing = await GetListingAsync(id, cancellationToken);

        listing.Unhide();
        await _listingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<ListingDto> ReplyAsync(int id, string content, CancellationToken cancellationToken)
    {
        var listing = await GetListingAsync(id, cancellationToken);
        var userId = _userContext.GetCurrentUserId().GetValueOrDefault();

        var reply = new Reply(id, userId, content);

        listing.AddReply(reply);
        await _listingRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ListingDto>(listing);
    }

    public async Task<IEnumerable<ReplyDto>> GetRepliesAsync(int id, CancellationToken cancellationToken)
    {
        var listing = await GetListingAsync(id, cancellationToken);
        var userId = _userContext.GetCurrentUserId().GetValueOrDefault();

        return _mapper.Map<IEnumerable<ReplyDto>>(listing.Replies);
    }

    private async Task<Listing> GetListingAsync(int id, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetCurrentUserId();
        var listing = await _listingRepository.FindAsync(l => l.Id == id && l.UserId == userId, cancellationToken);

        if (listing is null)
            throw new KeyNotFoundException("Listing not found.");

        return listing;
    }
}
