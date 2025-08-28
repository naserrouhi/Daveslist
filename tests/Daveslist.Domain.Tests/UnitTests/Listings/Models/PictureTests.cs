using Daveslist.Domain.Listings.Models;
using Daveslist.Domain.Shared.Constants;
using Daveslist.Domain.Shared.Exceptions;
using Xunit;

namespace Daveslist.Domain.Tests.UnitTests.Listings.Models;

public class PictureTests
{
    [Fact]
    public void Constructor_ShouldCreatePicture_WhenValidUrl()
    {
        // Arrange
        var url = "https://example.com/image.jpg";

        // Act
        var picture = new Picture(url);

        // Assert
        Assert.Equal(url, picture.Url);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowBusinessException_WhenUrlIsNullOrEmpty(string invalidUrl)
    {
        // Act & Assert
        var ex = Assert.Throws<BusinessException>(() => new Picture(invalidUrl));
        Assert.Contains("Picture url must be", ex.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessException_WhenUrlIsTooLong()
    {
        // Arrange
        var tooLongUrl = new string('a', DomainRulesConstants.Listing.MaxPictureUrlLength + 1);

        // Act & Assert
        var ex = Assert.Throws<BusinessException>(() => new Picture(tooLongUrl));
        Assert.Contains("Picture url must be", ex.Message);
    }
}
