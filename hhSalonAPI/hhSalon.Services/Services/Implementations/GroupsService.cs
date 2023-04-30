using hhSalon.Domain.Abstract;
using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalonAPI.Domain.Concrete;

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
                dbGroup.ImgUrl = group.ImgUrl;

                await _context.SaveChangesAsync();
            }
        }
    }
}
