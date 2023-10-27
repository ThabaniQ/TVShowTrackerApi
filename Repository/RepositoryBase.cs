using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TVShowTracker.Data;

namespace TVShowTracker.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext applicationContext { get; set; }
        public RepositoryBase(ApplicationDbContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }
        public IQueryable<T> FindAllAsync() => applicationContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByConditionAsync(Expression<Func<T, bool>> expression) =>
            applicationContext.Set<T>().Where(expression).AsNoTracking();

        public void CreateAsync(T entity) => applicationContext.Set<T>().Add(entity);
        public void UpdateAsync(T entity) => applicationContext.Set<T>().Update(entity);

        public void DeleteAsync(T entity) => applicationContext.Set<T>().Remove(entity);
    }
}
