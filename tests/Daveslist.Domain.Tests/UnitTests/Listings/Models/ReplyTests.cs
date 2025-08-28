using Daveslist.Domain.Listings.Models;
using Daveslist.Domain.Shared.Constants;
using Daveslist.Domain.Shared.Exceptions;
using Xunit;

namespace Daveslist.Domain.Tests.UnitTests.Listings.Models;

public class ReplyTests
{
    [Fact]
    public void Constructor_ShouldCreateReply_WhenValidParameters()
    {
        // Arrange
        var listingId = 1;
        var userId = 42;
        var content = "This is a reply";

        // Act
        var reply = new Reply(listingId, userId, content);

        // Assert
        Assert.Equal(listingId, reply.ListingId);
        Assert.Equal(userId, reply.UserId);
        Assert.Equal(content, reply.Content);

        var now = DateTime.UtcNow;
        Assert.True((now - reply.CreatedAt).TotalSeconds < 2, "CreatedAt should be set to current UTC time");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowBusinessException_WhenContentIsNullOrEmpty(string invalidContent)
    {
        // Act & Assert
        var ex = Assert.Throws<BusinessException>(() => new Reply(1, 42, invalidContent));
        Assert.Contains("Reply must be", ex.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessException_WhenContentIsTooLong()
    {
        // Arrange
        var tooLongContent = new string('a', DomainRulesConstants.Listing.MaxReplyLength + 1);

        // Act & Assert
        var ex = Assert.Throws<BusinessException>(() => new Reply(1, 42, tooLongContent));
        Assert.Contains("Reply must be", ex.Message);
    }
}
