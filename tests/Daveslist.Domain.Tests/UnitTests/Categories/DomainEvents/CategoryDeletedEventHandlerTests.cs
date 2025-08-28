using Daveslist.Domain.Categories.DomainEvents;
using Daveslist.Domain.Listings.Models;
using Daveslist.Domain.Listings.Repositories;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Daveslist.Domain.Tests.UnitTests.Categories.DomainEvents;

public class CategoryDeletedEventHandlerTests
{
    private readonly Mock<IListingRepository> _listingRepositoryMock;
    private readonly CategoryDeletedEventHandler _handler;

    public CategoryDeletedEventHandlerTests()
    {
        _listingRepositoryMock = new Mock<IListingRepository>();
        _handler = new CategoryDeletedEventHandler(_listingRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldMoveListingsToTrash_WhenCategoryDeleted()
    {
        // Arrange
        var categoryId = 10;
        var domainEvent = new CategoryDeletedEvent(categoryId);

        var listing1 = new Listing(userId: 1,
                                   categoryId: categoryId,
                                   title: "Title1",
                                   content: "Content1",
                                   isPublic: true,
                                   pictures: null);

        var listing2 = new Listing(userId: 2,
                                   categoryId: categoryId,
                                   title: "Title2",
                                   content: "Content2",
                                   isPublic: true,
                                   pictures: null);

        var listings = new List<Listing> { listing1, listing2 };

        _listingRepositoryMock
            .Setup(r => r.GetListAsync(It.IsAny<Expression<Func<Listing, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(listings);

        // Act
        await _handler.Handle(domainEvent, CancellationToken.None);

        // Assert
        listing1.IsTrashed.Should().BeTrue();
        listing2.IsTrashed.Should().BeTrue();
        listing1.CategoryId.Should().BeNull();
        listing2.CategoryId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldDoNothing_WhenNoListingsFound()
    {
        // Arrange
        var categoryId = 20;
        var domainEvent = new CategoryDeletedEvent(categoryId);

        _listingRepositoryMock
            .Setup(r => r.GetListAsync(It.IsAny<Expression<Func<Listing, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Listing>());

        // Act
        await _handler.Handle(domainEvent, CancellationToken.None);

        // Assert
        _listingRepositoryMock.Verify(r => r.GetListAsync(It.IsAny<Expression<Func<Listing, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUseCorrectExpression_ForFilteringByCategoryId()
    {
        // Arrange
        var categoryId = 30;
        var domainEvent = new CategoryDeletedEvent(categoryId);

        Expression<System.Func<Listing, bool>>? capturedExpr = null;

        _listingRepositoryMock
            .Setup(r => r.GetListAsync(It.IsAny<Expression<Func<Listing, bool>>>(), It.IsAny<CancellationToken>()))
            .Callback<Expression<Func<Listing, bool>>, CancellationToken>((expr, _) => capturedExpr = expr)
            .ReturnsAsync(new List<Listing>());

        // Act
        await _handler.Handle(domainEvent, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedExpr);
        var func = capturedExpr!.Compile();

        var matchingListing = new Listing(userId: 1,
                                          categoryId: categoryId,
                                          title: "TitleX",
                                          content: "ContentX",
                                          isPublic: true,
                                          pictures: null);

        var nonMatchingListing = new Listing(userId: 2,
                                             categoryId: 999,
                                             title: "TitleY",
                                             content: "ContentY",
                                             isPublic: true,
                                             pictures: null);

        Assert.True(func(matchingListing));
        Assert.False(func(nonMatchingListing));
    }
}
