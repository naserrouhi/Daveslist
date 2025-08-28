using Daveslist.Domain.Categories.Models;
using Daveslist.Domain.Categories.Repositories;
using Daveslist.Domain.Categories.Specifications;
using Daveslist.Domain.Shared.Exceptions;

namespace Daveslist.Domain.Categories.DomainServices;

public class CategoryDomainService : ICategoryDomainService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryDomainService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Category> CreateAsync(string name, bool isPublic, CancellationToken cancellationToken)
    {
        var expression = new DuplicateCategorySpecification(name).ToExpression();
        var isCategoryDuplicated = await _categoryRepository.AnyAsync(expression, cancellationToken);

        if (isCategoryDuplicated)
            throw new BusinessException("Category is duplicated.");

        var category = new Category(name, isPublic);
        await _categoryRepository.AddAsync(category, cancellationToken);

        return category;
    }
}
