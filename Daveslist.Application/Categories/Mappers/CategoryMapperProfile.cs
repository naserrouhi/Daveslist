using AutoMapper;
using Daveslist.Application.Categories.Models;
using Daveslist.Domain.Categories.Models;

namespace Daveslist.Application.Categories.Mappers;

public class CategoryMapperProfile : Profile
{
    public CategoryMapperProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}
