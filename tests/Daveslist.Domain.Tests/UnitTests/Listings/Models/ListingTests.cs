using Daveslist.Domain.Listings.Models;
using Daveslist.Domain.Shared.Constants;
using Daveslist.Domain.Shared.Exceptions;
using Xunit;

namespace Daveslist.Domain.Tests.UnitTests.Listings.Models;

public class ListingTests
{
    [Fact]
    public void Constructor_ShouldCreateListing_WhenValidData()
    {
        // Arrange
        var pictures = new List<Picture> { new Picture("url1") };

        // Act
        var listing = new Listing(1, 2, "Valid Title", "Valid Content", true, pictures);

        // Assert
        Assert.Equal(1, listing.UserId);
        Assert.Equal(2, listing.CategoryId);
        Assert.Equal("Valid Title", listing.Title);
        Assert.Equal("Valid Content", listing.Content);
        Assert.True(listing.IsPublic);
        Assert.False(listing.IsHidden);
        Assert.False(listing.IsTrashed);
        Assert.Equal(pictures, listing.Pictures);
        Assert.True((DateTime.UtcNow - listing.CreatedAt).TotalSeconds < 2);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenContentIsInvalid(string invalidContent)
    {
        Assert.Throws<BusinessException>(() => new Listing(1, 1, "Title", invalidContent, true, null));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenContentTooLong()
    {
        var content = new string('a', DomainRulesConstants.Listing.MaxContentLength + 1);

        Assert.Throws<BusinessException>(() => new Listing(1, 1, "Title", content, true, null));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenTitleIsInvalid(string invalidTitle)
    {
        Assert.Throws<BusinessException>(() => new Listing(1, 1, invalidTitle, "Content", true, null));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenTitleTooLong()
    {
        var title = new string('a', DomainRulesConstants.Listing.MaxTitleLength + 1);
        Assert.Throws<BusinessException>(() => new Listing(1, 1, title, "Content", true, null));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenTooManyPictures()
    {
        var pictures = new List<Picture>();
        for (int i = 0; i < DomainRulesConstants.Listing.MaximumPictureOfListing + 1; i++)
            pictures.Add(new Picture($"url{i}"));

        Assert.Throws<BusinessException>(() => new Listing(1, 1, "Title", "Content", true, pictures));
    }

    [Fact]
    public void MoveToTrash_ShouldSetFlags()
    {
        var listing = new Listing(1, 1, "Title", "Content", true, null);
        listing.MoveToTrash();

        Assert.True(listing.IsTrashed);
        Assert.Null(listing.CategoryId);
    }

    [Fact]
    public void HideAndUnhide_ShouldToggleIsHidden()
    {
        var listing = new Listing(1, 1, "Title", "Content", true, null);

        listing.Hide();
        Assert.True(listing.IsHidden);

        listing.Unhide();
        Assert.False(listing.IsHidden);
    }

    [Fact]
    public void Update_ShouldChangeProperties()
    {
        var listing = new Listing(1, 1, "OldTitle", "OldContent", true, null);

        var newPictures = new List<Picture> { new Picture("newUrl") };
        listing.Update(99, "NewTitle", "NewContent", false, newPictures);

        Assert.Equal(99, listing.CategoryId);
        Assert.Equal("NewTitle", listing.Title);
        Assert.Equal("NewContent", listing.Content);
        Assert.False(listing.IsPublic);
        Assert.Equal(newPictures, listing.Pictures);
    }
}
