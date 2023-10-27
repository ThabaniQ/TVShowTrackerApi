using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAllAsync();
    IQueryable<T> FindByConditionAsync(Expression<Func<T, bool>> expression);
    void CreateAsync(T entity);
    void UpdateAsync(T entity);
    void DeleteAsync(T entity);
}
