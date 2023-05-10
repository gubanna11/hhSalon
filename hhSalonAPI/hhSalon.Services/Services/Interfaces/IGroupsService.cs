using hhSalon.Domain.Abstract;
using hhSalon.Domain.Entities;

namespace hhSalon.Services.Services.Interfaces
{
    public interface IGroupsService : IEntityBaseRepository<GroupOfServices>
    {
        Task UpdateGroupAsync(GroupOfServices group);

        Task<List<GroupOfServices>> GetGroupsByWorkerId(string workerId);

	}
}
