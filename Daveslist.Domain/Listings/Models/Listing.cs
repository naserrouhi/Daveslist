using Daveslist.Domain.Shared.Constants;
using Daveslist.Domain.Shared.Exceptions;
using Daveslist.Domain.Shared.Models;

namespace Daveslist.Domain.Listings.Models;

public class Listing : AggregateRoot<int>
{
    protected Listing() { }

    public Listing(int userId,
                   int categoryId,
                   string title,
                   string content,
                   bool isPublic,
                   List<Picture>? pictures)
    {
        if (string.IsNullOrWhiteSpace(content) || content.Length > DomainRulesConstants.Listing.MaxContentLength)
            throw new BusinessException($"Listing content must be 1-{DomainRulesConstants.Listing.MaxContentLength} characters.");

        if (string.IsNullOrWhiteSpace(title) || title.Length > DomainRulesConstants.Listing.MaxTitleLength)
            throw new BusinessException($"Listing title must be 1-{DomainRulesConstants.Listing.MaxTitleLength} characters.");

        if ((pictures?.Count ?? 0) > DomainRulesConstants.Listing.MaximumPictureOfListing)
            throw new BusinessException($"Maximum of {DomainRulesConstants.Listing.MaximumPictureOfListing} images allowed per listing.");

        UserId = userId;
        CategoryId = categoryId;
        Title = title;
        Content = content;
        IsPublic = isPublic;
        IsHidden = false;
        IsTrashed = false;
        CreatedAt = DateTime.UtcNow;
        Pictures = pictures;
    }

    public int UserId { get; private set; }
    public int? CategoryId { get; private set; }
    public bool IsPublic { get; private set; }
    public bool IsHidden { get; private set; }
    public bool IsTrashed { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }

    public ICollection<Picture>? Pictures { get; private set; }
    public ICollection<Reply> Replies { get; private set; }

    public void AddReply(Reply reply)
    {
        if (CreatedAt < DateTime.UtcNow.AddYears(-1))
            throw new BusinessException("Replies are only allowed for listings created within the past year.");

        Replies.Add(reply);
    }

    public void MoveToTrash()
    {
        IsTrashed = true;
        CategoryId = null;
    }

    public void Hide() => IsHidden = true;
    public void Unhide() => IsHidden = false;

    public void Update(int categoryId,
                       string title,
                       string content,
                       bool isPublic,
                       List<Picture>? pictures)
    {
        CategoryId = categoryId;
        Title = title;
        Content = content;
        IsPublic = isPublic;
        Pictures = pictures;
    }
}
