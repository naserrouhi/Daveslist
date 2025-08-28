using Daveslist.Application.Categories.AppServices;
using Daveslist.Domain.Categories.Repositories;
using Daveslist.TestBase.BaseClasses;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Daveslist.Application.Tests.IntegrationTests.Categories.AppServices;

public class CategoryAppServiceTests : BaseIntegrationTest
{
    private readonly ICategoryAppService _categoryAppService;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryAppServiceTests()
    {
        _categoryAppService = _serviceProvider.GetRequiredService<ICategoryAppService>();
        _categoryRepository = _serviceProvider.GetRequiredService<ICategoryRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCategory()
    {
        // Arrange
        var name = "Test Category";
        var isPublic = true;

        // Act
        var result = await _categoryAppService.CreateAsync(name, isPublic, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.IsPublic.Should().Be(isPublic);

        var dbCategory = await _categoryRepository.FindAsync(c => c.Id == result.Id, CancellationToken.None);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(name);
    }

    [Fact]
    public async Task GetListAsync_WhenUserAuthenticated_ShouldReturnAllCategories()
    {
        // Arrange
        await _categoryAppService.CreateAsync("Public Cat", true, CancellationToken.None);
        await _categoryAppService.CreateAsync("Private Cat", false, CancellationToken.None);

        // Act
        var result = await _categoryAppService.GetListAsync(isUserAuthenticated: true, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetListAsync_WhenUserNotAuthenticated_ShouldReturnOnlyPublicCategories()
    {
        // Arrange
        await _categoryAppService.CreateAsync("Public Cat", true, CancellationToken.None);
        await _categoryAppService.CreateAsync("Private Cat", false, CancellationToken.None);

        // Act
        var result = await _categoryAppService.GetListAsync(isUserAuthenticated: false, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().IsPublic.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCategory()
    {
        // Arrange
        var category = await _categoryAppService.CreateAsync("To Delete", true, CancellationToken.None);

        // Act
        await _categoryAppService.DeleteAsync(category.Id, CancellationToken.None);

        // Assert
        var dbCategory = await _categoryRepository.FindAsync(c => c.Id == category.Id, CancellationToken.None);
        dbCategory.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenCategoryNotFound_ShouldThrow()
    {
        // Act
        var act = async () => await _categoryAppService.DeleteAsync(999, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Category not found.");
    }
}
