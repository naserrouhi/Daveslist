using Daveslist.Domain.Categories.DomainServices;
using Daveslist.Domain.Categories.Models;
using Daveslist.Domain.Categories.Repositories;
using Daveslist.Domain.Shared.Exceptions;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Daveslist.Domain.Tests.UnitTests.Categories.DomainServices;

public class CategoryDomainServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly CategoryDomainService _service;

    public CategoryDomainServiceTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _service = new CategoryDomainService(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCategory_WhenNotDuplicated()
    {
        // Arrange
        var name = "Sport";
        var isPublic = true;

        _categoryRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.CreateAsync(name, isPublic, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(isPublic, result.IsPublic);
        _categoryRepositoryMock.Verify(r => r.AddAsync(It.Is<Category>(c => c.Name == name), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowBusinessException_WhenCategoryIsDuplicated()
    {
        // Arrange
        var name = "Sport";
        _categoryRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(name, true, CancellationToken.None));
        Assert.Equal("Category is duplicated.", ex.Message);

        _categoryRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldPassCorrectExpression_ToRepository()
    {
        // Arrange
        var name = "Sport";
        Expression<Func<Category, bool>>? capturedExpr = null;

        _categoryRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
            .Callback<Expression<Func<Category, bool>>, CancellationToken>((expr, _) => capturedExpr = expr)
            .ReturnsAsync(false);

        // Act
        await _service.CreateAsync(name, true, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedExpr);
        var func = capturedExpr!.Compile();
        var category = new Category(name, true);
        Assert.True(func(category));
    }
}
