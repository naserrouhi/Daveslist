using Daveslist.Domain.Categories.Models;
using Daveslist.Domain.Shared.Models;
using System.Linq.Expressions;

namespace Daveslist.Domain.Categories.Specifications;

public class DuplicateCategorySpecification : Specification<Category>
{
    private readonly string _name;

    public DuplicateCategorySpecification(string name)
    {
        _name = name;
    }

    public override Expression<Func<Category, bool>> ToExpression()
    {
        return c => c.Name == _name;
    }
}
