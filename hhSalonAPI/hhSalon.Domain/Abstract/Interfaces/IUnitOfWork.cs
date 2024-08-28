namespace hhSalon.Domain.Abstract.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GenericRepository<T>() where T : EntityBase, new();
        Task SaveChangesAsync();
    }
}
