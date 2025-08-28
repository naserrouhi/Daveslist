using Daveslist.Domain.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace Daveslist.Application.Listings.Models;

public record UpsertListingDto
{
    [Required, MaxLength(DomainRulesConstants.Listing.MaxTitleLength)]
    public string? Title { get; init; }

    [Required, MaxLength(DomainRulesConstants.Listing.MaxContentLength)]
    public string? Content { get; init; }

    [Required]
    public int CategoryId { get; init; }

    [Required]
    public bool IsPublic { get; init; }

    public IEnumerable<string>? PictureUrls { get; init; }
}
