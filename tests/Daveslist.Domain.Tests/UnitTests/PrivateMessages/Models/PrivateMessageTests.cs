using Daveslist.Domain.PrivateMessages.Models;
using Daveslist.Domain.Shared.Constants;
using Daveslist.Domain.Shared.Exceptions;
using Xunit;

namespace Daveslist.Domain.Tests.UnitTests.PrivateMessages.Models;

public class PrivateMessageTests
{
    [Fact]
    public void Constructor_ShouldCreatePrivateMessage_WhenValidParameters()
    {
        // Arrange
        var fromUserId = 1;
        var toUserId = 2;
        var content = "Hello, how are you?";

        // Act
        var message = new PrivateMessage(fromUserId, toUserId, content);

        // Assert
        Assert.Equal(fromUserId, message.FromUserId);
        Assert.Equal(toUserId, message.ToUserId);
        Assert.Equal(content, message.Content);

        var now = DateTime.UtcNow;
        Assert.True((now - message.SentAt).TotalSeconds < 2, "SentAt should be close to current UTC time");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowBusinessException_WhenContentIsNullOrEmpty(string invalidContent)
    {
        // Act & Assert
        var ex = Assert.Throws<BusinessException>(() => new PrivateMessage(1, 2, invalidContent));
        Assert.Contains("Private message must be", ex.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessException_WhenContentIsTooLong()
    {
        // Arrange
        var tooLongContent = new string('a', DomainRulesConstants.PrivateMessage.MaxContentLength + 1);

        // Act & Assert
        var ex = Assert.Throws<BusinessException>(() => new PrivateMessage(1, 2, tooLongContent));
        Assert.Contains("Private message must be", ex.Message);
    }
}
