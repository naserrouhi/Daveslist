using Daveslist.Domain.Categories.Models;
using Daveslist.Domain.Categories.Specifications;
using Xunit;

namespace Daveslist.Domain.Tests.UnitTests.Categories.Specifications;

public class DuplicateCategorySpecificationTests
{
    [Fact]
    public void ToExpression_ShouldReturnTrue_WhenCategoryHasSameName()
    {
        // Arrange
        var categoryName = "Sport";
        var category = new Category(categoryName, true);
        var spec = new DuplicateCategorySpecification(categoryName);

        // Act
        var expression = spec.ToExpression();
        var func = expression.Compile();
        var result = func(category);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ToExpression_ShouldReturnFalse_WhenCategoryHasDifferentName()
    {
        // Arrange
        var category = new Category("Movies", true);
        var spec = new DuplicateCategorySpecification("Sport");

        // Act
        var expression = spec.ToExpression();
        var func = expression.Compile();
        var result = func(category);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("Sport", "Sport", true)]
    [InlineData("Sport", "books", false)]
    [InlineData("Sport", "Other", false)]
    public void ToExpression_ShouldMatchExpectedResults(string specName, string categoryName, bool expected)
    {
        // Arrange
        var category = new Category(categoryName, true);
        var spec = new DuplicateCategorySpecification(specName);

        // Act
        var expression = spec.ToExpression();
        var func = expression.Compile();
        var result = func(category);

        // Assert
        Assert.Equal(expected, result);
    }
}
