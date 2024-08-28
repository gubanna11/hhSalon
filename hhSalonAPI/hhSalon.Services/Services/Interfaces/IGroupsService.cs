using hhSalon.Domain.Abstract.Interfaces;
using hhSalon.Domain.Entities;

namespace hhSalon.Services.Services.Interfaces
{
    public interface IGroupsService
    {
        Task UpdateGroupAsync(GroupOfServices group);

        Task<List<GroupOfServices>> GetGroupsByWorkerId(string workerId);
        Task AddAsync(GroupOfServices groupOfServices);
        Task<IEnumerable<GroupOfServices>> GetAllAsync();
        Task<GroupOfServices> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
