namespace Daveslist.Application.Listings.Models;

public record ListingDto
{
    public int Id { get; set; }
    public int? CategoryId { get; init; }
    public bool IsPublic { get; init; }
    public bool IsHidden { get; init; }
    public bool IsTrashed { get; init; }
    public DateTime CreatedAt { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }

    public IEnumerable<PictureDto> Pictures { get; init; } = [];
    public IEnumerable<ReplyDto> Replies { get; init; } = [];
}

public record PictureDto(string Url);

public record ReplyDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Content { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
