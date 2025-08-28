using AutoMapper;
using Daveslist.Application.PrivateMessages.AppServices;
using Daveslist.Application.PrivateMessages.Models;
using Daveslist.Domain.PrivateMessages.Models;
using Daveslist.Domain.PrivateMessages.Repositories;
using Daveslist.Domain.Users.Infrastructures;
using Daveslist.Domain.Users.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Daveslist.Application.Tests.IntegrationTests.PrivateMessages.AppServices;

public class PrivateMessageAppServiceTests
{
    private readonly Mock<IPrivateMessageRepository> _messageRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IIdentityManager> _identityManagerMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly PrivateMessageAppService _messageAppService;

    public PrivateMessageAppServiceTests()
    {
        _messageRepositoryMock = new Mock<IPrivateMessageRepository>();
        _userContextMock = new Mock<IUserContext>();
        _identityManagerMock = new Mock<IIdentityManager>();
        _mapperMock = new Mock<IMapper>();

        _messageAppService = new PrivateMessageAppService(
            _messageRepositoryMock.Object,
            _userContextMock.Object,
            _mapperMock.Object,
            _identityManagerMock.Object
        );
    }

    [Fact]
    public async Task SendAsync_ShouldSendMessage()
    {
        // Arrange
        var fromUserId = 1;
        var toUserId = 2;
        var content = "Hello!";

        _userContextMock.Setup(c => c.GetCurrentUserId()).Returns(fromUserId);
        _identityManagerMock.Setup(m => m.FindByIdAsync(toUserId))
                            .ReturnsAsync(new UserModel("username", "email", "firstName", "lastName"));

        // Act
        await _messageAppService.SendAsync(toUserId, content, CancellationToken.None);

        // Assert
        _messageRepositoryMock.Verify(r =>
            r.AddAsync(It.Is<PrivateMessage>(m =>
                m.FromUserId == fromUserId &&
                m.ToUserId == toUserId &&
                m.Content == content),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _messageRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendAsync_ShouldThrow_WhenRecipientNotFound()
    {
        // Arrange
        var fromUserId = 1;
        var toUserId = 999;
        _userContextMock.Setup(c => c.GetCurrentUserId()).Returns(fromUserId);
        _identityManagerMock.Setup(m => m.FindByIdAsync(toUserId))
                            .ReturnsAsync((UserModel?)null);

        // Act
        var act = async () => await _messageAppService.SendAsync(toUserId, "Hi!", CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("User not found.");
        _messageRepositoryMock.Verify(r => r.AddAsync(It.IsAny<PrivateMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetListAsync_ShouldReturnMessagesForCurrentUser()
    {
        // Arrange
        var userId = 1;
        _userContextMock.Setup(c => c.GetCurrentUserId()).Returns(userId);

        var messages = new List<PrivateMessage>
        {
            new(1, 1, "Message 1"),
            new PrivateMessage(1, 2, "Message 2")
        };

        _messageRepositoryMock.Setup(r => r.GetListAsync(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<PrivateMessage, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(messages);

        _mapperMock.Setup(m => m.Map<IEnumerable<PrivateMessageDto>>(messages))
                   .Returns(messages.Select(msg => new PrivateMessageDto(msg.FromUserId, msg.ToUserId, msg.Content, DateTime.Now)));

        // Act
        var result = await _messageAppService.GetListAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Select(m => m.Content).Should().Contain(new[] { "Message 1", "Message 2" });
    }
}
