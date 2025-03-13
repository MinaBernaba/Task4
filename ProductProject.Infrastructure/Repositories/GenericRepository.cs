using LinqKit;
using Microsoft.EntityFrameworkCore;
using ProductProject.Infrastructure.Context;
using ProductProject.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace ProductProject.Infrastructure.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
    {
        public virtual async Task<IReadOnlyList<T>> GetAllAsync() => await context.Set<T>().AsNoTracking().ToListAsync();
        public virtual IQueryable<T> GetAllAsNoTracking() => context.Set<T>().AsNoTracking();

        public virtual async Task<bool> IsExistAsync(params Expression<Func<T, bool>>[] conditions)
        {
            var predicate = PredicateBuilder.New<T>();

            foreach (var condition in conditions)
                predicate = predicate.And(condition);

            return await context.Set<T>().AnyAsync(predicate);
        }
        public virtual async Task<T> GetByIdAsync(int id) => (await context.Set<T>().FindAsync(id))!;
        public virtual async Task AddAsync(T entity) => await context.Set<T>().AddAsync(entity);
        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await context.Set<T>().AddRangeAsync(entities);
        public virtual void Update(T entity) => context.Set<T>().Update(entity);
        public virtual void UpdateRange(IEnumerable<T> entities) => context.Set<T>().UpdateRange(entities);
        public virtual void Delete(T entity) => context.Set<T>().Remove(entity);
        public virtual void DeleteRange(IEnumerable<T> entities) => context.Set<T>().RemoveRange(entities);
    }
}