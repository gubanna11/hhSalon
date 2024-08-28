using hhSalon.Domain.Abstract.Interfaces;
using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hhSalon.Services.Services.Implementations
{
    public class GroupsService : IGroupsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateGroupAsync(GroupOfServices group)
        {
            var dbGroup = await _unitOfWork.GenericRepository<GroupOfServices>().GetByIdAsync(group.Id);
            if (dbGroup != null)
            {
                dbGroup.Name = group.Name;

                if (_unitOfWork.GenericRepository<GroupOfServices>().Set.Where(g => g.Name == group.Name).Count() > 1)
                {
                    throw new DbUpdateException("Group already exists!");
                }

                dbGroup.ImgUrl = group.ImgUrl;

                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<List<GroupOfServices>> GetGroupsByWorkerId(string workerId)
        {
            var groups = await _unitOfWork
                .GenericRepository<WorkerGroup>()
                .Set
                .Where(wg => wg.WorkerId == workerId)
                .Select(wg => wg.Group)
                .ToListAsync();
            return groups;
        }

        public async Task AddAsync(GroupOfServices groupOfServices)
        {
            await _unitOfWork.GenericRepository<GroupOfServices>().AddAsync(groupOfServices);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<GroupOfServices>> GetAllAsync()
        {
            IEnumerable<GroupOfServices> groups = await _unitOfWork
                .GenericRepository<GroupOfServices>()
                .GetAllAsync();
            return groups;
        }

        public async Task<GroupOfServices> GetByIdAsync(int id)
        {
            GroupOfServices group = await _unitOfWork
                .GenericRepository<GroupOfServices>()
                .GetByIdAsync(id);
            return group;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GenericRepository<GroupOfServices>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
