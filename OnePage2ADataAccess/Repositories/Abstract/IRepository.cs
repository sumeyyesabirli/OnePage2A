using System.Linq.Expressions;

namespace OnePage2ADataAccess.Repositories.Abstract
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
    }
}
