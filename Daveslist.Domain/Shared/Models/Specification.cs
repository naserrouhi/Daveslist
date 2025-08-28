using System.Linq.Expressions;

namespace Daveslist.Domain.Shared.Models;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();
}
