using hhSalon.Domain.Abstract.Interfaces;
using hhSalonAPI.Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace hhSalon.Domain.Abstract.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase, new()
    {
        private readonly AppDbContext _context;
        public IQueryable<T> Set { get; }

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            Set = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(n => n.Id == id);
            EntityEntry entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
            => await _context.Set<T>().FirstOrDefaultAsync(n => n.Id == id);


        public async Task UpdateAsync(T entity)
        {
            EntityEntry entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;
        }
    }
}
