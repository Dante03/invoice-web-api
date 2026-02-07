using Humanizer;
using invoice_web_api.Data;
using invoice_web_api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace invoice_web_api.Repositories
{
    public class GenericRepository<T, TDto>
    where T : class, new()
    where TDto : class, new()
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual Task<T?> Populate(TDto dto)
        {
            if (dto == null)
                return Task.FromResult<T?>(null);

            var entity = new T();

            return Task.FromResult(entity);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Remove(T entity)
            => _dbSet.Remove(entity);
    }
}
