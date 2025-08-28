using AutoMapper;
using Daveslist.Application.Categories.Models;
using Daveslist.Domain.Categories.DomainEvents;
using Daveslist.Domain.Categories.DomainServices;
using Daveslist.Domain.Categories.Repositories;
using Daveslist.Domain.Shared.Interfaces.DomainEvents;

namespace Daveslist.Application.Categories.AppServices;

public class CategoryAppService : ICategoryAppService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryDomainService _categoryDomainService;
    private readonly IMapper _mapper;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public CategoryAppService(ICategoryRepository categoryRepository,
                              ICategoryDomainService categoryDomainService,
                              IMapper mapper,
                              IDomainEventDispatcher domainEventDispatcher)
    {
        _categoryRepository = categoryRepository;
        _categoryDomainService = categoryDomainService;
        _mapper = mapper;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<IEnumerable<CategoryDto>> GetListAsync(CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> CreateAsync(string name, bool isPublic, CancellationToken cancellationToken)
    {
        var category = await _categoryDomainService.CreateAsync(name, isPublic, cancellationToken);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.FindAsync(c => c.Id == id, cancellationToken);

        if (category is null)
            throw new KeyNotFoundException("Category not found.");

        await _domainEventDispatcher.DispatchAsync(new CategoryDeletedEvent(id), cancellationToken);

        _categoryRepository.Remove(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);
    }
}
