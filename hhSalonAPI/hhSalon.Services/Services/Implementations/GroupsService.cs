using hhSalon.Domain.Abstract;
using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalonAPI.Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace hhSalon.Services.Services.Implementations
{
    public class GroupsService : EntityBaseRepository<GroupOfServices>, IGroupsService
    {
        private readonly AppDbContext _context;

        public GroupsService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateGroupAsync(GroupOfServices group)
        {
            var dbGroup = await GetByIdAsync(group.Id);
            if (dbGroup != null)
            {
                dbGroup.Name = group.Name;

                if (_context.Groups.Where(g => g.Name == group.Name).Count() > 1)
                {
                    throw new DbUpdateException("Group already exists!");
                }               

                dbGroup.ImgUrl = group.ImgUrl;

                await _context.SaveChangesAsync();
            }
        }

		public async Task<List<GroupOfServices>> GetGroupsByWorkerId(string workerId)
        {
            var groups = await _context.Workers_Groups.Where(wg => wg.WorkerId == workerId).Select(wg => wg.Group).ToListAsync();

            return groups;
		}
	}
}
