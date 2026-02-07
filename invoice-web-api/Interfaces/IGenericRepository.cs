using System.Linq.Expressions;

namespace invoice_web_api.Interfaces
{
    public interface IGenericRepository<T, TDto> where T : class, new()
    {
        Task<T?> Populate(TDto dto);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
