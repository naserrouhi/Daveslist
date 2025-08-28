using Daveslist.Domain.Shared.Constants;
using Daveslist.Domain.Shared.Exceptions;

namespace Daveslist.Domain.Listings.Models;

public record Picture
{
    protected Picture() { }

    public Picture(string url)
    {
        if (string.IsNullOrWhiteSpace(url) || url.Length > DomainRulesConstants.Listing.MaxPictureUrlLength)
            throw new BusinessException($"Picture url must be 1-{DomainRulesConstants.Listing.MaxPictureUrlLength} characters.");

        Url = url;
    }

    public string Url { get; private set; }
}
