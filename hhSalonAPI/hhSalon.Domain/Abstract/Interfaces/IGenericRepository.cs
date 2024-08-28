using System.Linq.Expressions;

namespace hhSalon.Domain.Abstract.Interfaces
{
    public interface IGenericRepository<T> where T : EntityBase, new()
    {
        public IQueryable<T> Set { get; }
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
