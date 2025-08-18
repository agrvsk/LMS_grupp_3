using System.Linq.Expressions;

namespace Domain.Contracts.Repositories;

public interface IInternalRepositoryBase<T>
{
    Task<IQueryable<T>> FindAllAsync(bool trackChanges = false);
    Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges = false);
}
