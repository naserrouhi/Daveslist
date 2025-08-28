using Daveslist.Application.Categories.AppServices;
using Daveslist.Application.Categories.Models;
using Daveslist.Application.Listings.AppServices;
using Daveslist.Application.Listings.Models;
using Daveslist.Domain.Listings.Repositories;
using Daveslist.Domain.Users.Infrastructures;
using Daveslist.TestBase.BaseClasses;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Daveslist.Application.Tests.IntegrationTests.Listings.AppServices;

public class ListingAppServiceTests : BaseIntegrationTest
{
    private readonly IListingAppService _listingAppService;
    private readonly ICategoryAppService _categoryAppService;
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public ListingAppServiceTests()
    {
        _listingAppService = _serviceProvider.GetRequiredService<IListingAppService>();
        _categoryAppService = _serviceProvider.GetRequiredService<ICategoryAppService>();
        _listingRepository = _serviceProvider.GetRequiredService<IListingRepository>();
        _userContext = _serviceProvider.GetRequiredService<IUserContext>();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateListing()
    {
        // Arrange
        var category = await CreateCategoryAsync();
        var dto = BuildUpsertDto(category.Id);

        // Act
        var listing = await _listingAppService.CreateAsync(dto, CancellationToken.None);

        // Assert
        listing.Should().NotBeNull();
        listing.Title.Should().Be(dto.Title);

        var dbListing = await _listingRepository.FindAsync(l => l.Id == listing.Id, CancellationToken.None);
        dbListing.Should().NotBeNull();
    }

    [Fact]
    public async Task GetListAsync_ShouldReturnListings_WhenCategoryExists()
    {
        // Arrange
        var category = await CreateCategoryAsync();
        var dto = BuildUpsertDto(category.Id);
        await _listingAppService.CreateAsync(dto, CancellationToken.None);

        // Act
        var listings = await _listingAppService.GetListAsync(true, category.Id, 1, 10, CancellationToken.None);

        // Assert
        listings.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetListAsync_ShouldThrow_WhenCategoryDoesNotExist()
    {
        // Act
        var act = async () => await _listingAppService.GetListAsync(true, 999, 1, 10, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Category not found.");
    }

    [Fact]
    public async Task GetAsync_ShouldReturnListing_WhenExists()
    {
        // Arrange
        var category = await CreateCategoryAsync();
        var dto = BuildUpsertDto(category.Id);
        var created = await _listingAppService.CreateAsync(dto, CancellationToken.None);

        // Act
        var listing = await _listingAppService.GetAsync(true, created.Id, CancellationToken.None);

        // Assert
        listing.Should().NotBeNull();
        listing.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateListing()
    {
        // Arrange
        var category = await CreateCategoryAsync();
        var dto = BuildUpsertDto(category.Id);
        var created = await _listingAppService.CreateAsync(dto, CancellationToken.None);

        var updateDto = BuildUpsertDto(category.Id, "Updated title", "Updated content");

        // Act
        var updated = await _listingAppService.UpdateAsync(created.Id, updateDto, isAdmin: false, CancellationToken.None);

        // Assert
        updated.Title.Should().Be("Updated title");
        updated.Content.Should().Be("Updated content");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveListing()
    {
        // Arrange
        var category = await CreateCategoryAsync();
        var dto = BuildUpsertDto(category.Id);
        var created = await _listingAppService.CreateAsync(dto, CancellationToken.None);

        // Act
        await _listingAppService.DeleteAsync(created.Id, isAdmin: false, CancellationToken.None);

        // Assert
        var dbListing = await _listingRepository.FindAsync(l => l.Id == created.Id, CancellationToken.None);
        dbListing.Should().BeNull();
    }

    [Fact]
    public async Task HideAndUnhide_ShouldToggleVisibility()
    {
        // Arrange
        var category = await CreateCategoryAsync();
        var dto = BuildUpsertDto(category.Id);
        var created = await _listingAppService.CreateAsync(dto, CancellationToken.None);

        // Act
        await _listingAppService.HideAsync(created.Id, CancellationToken.None);
        var hidden = await _listingRepository.FindAsync(l => l.Id == created.Id, CancellationToken.None);

        await _listingAppService.UnhideAsync(created.Id, CancellationToken.None);
        var unhidden = await _listingRepository.FindAsync(l => l.Id == created.Id, CancellationToken.None);

        // Assert
        hidden!.IsHidden.Should().BeTrue();
        unhidden!.IsHidden.Should().BeFalse();
    }

    [Fact]
    public async Task ReplyAsync_ShouldAddReplyToListing()
    {
        // Arrange
        var category = await CreateCategoryAsync();
        var dto = BuildUpsertDto(category.Id);
        var created = await _listingAppService.CreateAsync(dto, CancellationToken.None);

        // Act
        var listingWithReply = await _listingAppService.ReplyAsync(created.Id, "This is a reply", CancellationToken.None);

        // Assert
        listingWithReply.Replies.Should().ContainSingle(r => r.Content == "This is a reply");
    }

    private async Task<CategoryDto> CreateCategoryAsync(string name = "Default Cat", bool isPublic = true)
    {
        return await _categoryAppService.CreateAsync(name, isPublic, CancellationToken.None);
    }

    private UpsertListingDto BuildUpsertDto(int categoryId, string title = "Listing title", string content = "Listing content", bool isPublic = true)
    {
        return new UpsertListingDto
        {
            CategoryId = categoryId,
            Title = title,
            Content = content,
            IsPublic = isPublic,
            PictureUrls = []
        };
    }
}
