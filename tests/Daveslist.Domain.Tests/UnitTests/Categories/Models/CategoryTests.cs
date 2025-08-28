using Daveslist.Domain.Categories.Models;
using Daveslist.Domain.Shared.Constants;
using Daveslist.Domain.Shared.Exceptions;
using Xunit;

namespace Daveslist.Domain.Tests.UnitTests.Categories.Models;

public class CategoryTests
{
    [Fact]
    public void Constructor_ShouldCreateCategory_WhenValidParameters()
    {
        // Arrange
        var name = "ValidCategory";
        var isPublic = true;

        // Act
        var category = new Category(name, isPublic);

        // Assert
        Assert.Equal(name, category.Name);
        Assert.Equal(isPublic, category.IsPublic);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowBusinessException_WhenNameIsNullOrEmpty(string invalidName)
    {
        // Act & Assert
        var ex = Assert.Throws<BusinessException>(() => new Category(invalidName, true));
        Assert.Contains("Category name must be", ex.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessException_WhenNameIsTooLong()
    {
        // Arrange
        var tooLongName = new string('a', DomainRulesConstants.Category.MaxNameLength + 1);

        // Act & Assert
        var ex = Assert.Throws<BusinessException>(() => new Category(tooLongName, false));
        Assert.Contains("Category name must be", ex.Message);
    }
}
