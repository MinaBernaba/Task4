using System.Linq.Expressions;

namespace ProductProject.Application.ServiceInterfaces
{
    public interface IBaseService<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> IsExistAsync(int id);
        Task<bool> IsExistAsync(params Expression<Func<T, bool>>[] conditions);
        Task<bool> AddAsync(T entity);
        Task<bool> AddRangeAsync(IEnumerable<T> entites);
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateRangeAsync(IEnumerable<T> entites);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteRangeAsync(HashSet<int> ids);
    }
}
