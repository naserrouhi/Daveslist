using Daveslist.Domain.Shared.Constants;
using Daveslist.Domain.Shared.Exceptions;
using Daveslist.Domain.Shared.Models;

namespace Daveslist.Domain.Listings.Models;

public class Reply : Entity<int>
{
    protected Reply() { }

    public Reply(int listingId, int userId, string content)
    {
        if (string.IsNullOrWhiteSpace(content) || content.Length > DomainRulesConstants.Listing.MaxReplyLength)
            throw new BusinessException($"Reply must be 1-{DomainRulesConstants.Listing.MaxReplyLength} characters.");

        ListingId = listingId;
        UserId = userId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }

    public int ListingId { get; private set; }
    public int UserId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
}
