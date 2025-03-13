using System.Linq.Expressions;

namespace ProductProject.Infrastructure.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        IQueryable<T> GetAllAsNoTracking();
        Task<bool> IsExistAsync(params Expression<Func<T, bool>>[] conditions);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
