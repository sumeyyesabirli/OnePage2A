using Microsoft.EntityFrameworkCore;
using OnePage2ADataAccess.Contexts;
using OnePage2ADataAccess.Repositories.Abstract;
using System.Linq.Expressions;

namespace OnePage2ADataAccess.Repositories.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext2A _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext2A context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? await _dbSet.ToListAsync() : await _dbSet.Where(filter).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(filter);
        }
    }
}
