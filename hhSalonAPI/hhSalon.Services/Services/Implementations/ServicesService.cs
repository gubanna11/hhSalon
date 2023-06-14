using hhSalon.Domain.Abstract;
using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Domain.Concrete;
using Microsoft.EntityFrameworkCore;

namespace hhSalon.Services.Services.Implementations
{
    public class ServicesService : EntityBaseRepository<Service>, IServicesService
    {
        private readonly AppDbContext _context;
        public ServicesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ServiceVM>> GetServicesByGroupIdAsync(int groupId)
        {
            var services = await _context.Services.Include(s => s.ServiceGroup).Where(s => s.ServiceGroup.GroupId == groupId).ToListAsync();

            if( services is null || services.Count == 0)
            {
                throw new Exception("Empty");
            }

            List<ServiceVM> servicesVM = new List<ServiceVM>();

            foreach (var service in services)
            {
                ServiceVM serviceVM = new ServiceVM()
                {
                    Id = service.Id,
                    Name = service.Name,
                    Price = service.Price,
                    GroupId = service.ServiceGroup.GroupId
                };
                servicesVM.Add(serviceVM);
            }
            return servicesVM;
        
        }


        public async Task AddNewServiceAsync(ServiceVM newService)
        {
            Service service = new Service
            {
                Name = newService.Name,
                Price = newService.Price
            };

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();

            ServiceGroup serviceGroup = new ServiceGroup
            {
                ServiceId = service.Id,
                GroupId = newService.GroupId
            };

            await _context.Services_Groups.AddAsync(serviceGroup);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateServiceAsync(ServiceVM serviceVM)
        {
            //var service = await GetByIdAsync(serviceVM.Id);
            var service = _context.Services.Where(s => s.Id == serviceVM.Id).Include(s => s.ServiceGroup).FirstOrDefault();
            
            if (service != null)
            {
                
                service.Name = serviceVM.Name;
                service.Price = serviceVM.Price;

                if (service.ServiceGroup.GroupId != serviceVM.GroupId)
                {
                    var service_group = _context.Services_Groups.Where(sg => sg.ServiceId == service.Id).FirstOrDefault();
                    _context.Services_Groups.Remove(service_group);

                    ServiceGroup newService_Group = new ServiceGroup()
                    {
                        ServiceId = service.Id,
                        GroupId = serviceVM.GroupId
                    };

                    await _context.Services_Groups.AddAsync(newService_Group);
                }

                await _context.SaveChangesAsync();
            }
            else
                throw new NullReferenceException();           
        }

        public async Task<ServiceVM> GetServiceVMByIdAsync(int id)
        {

			//var service = await _context.Services.Where(s => s.Id == id)
			//	.Include(s => s.Service_Group).ThenInclude(sg => sg.Group).FirstOrDefaultAsync();


			var service = await _context.Services.Where(s => s.Id == id).FirstOrDefaultAsync();

            if(service == null)
                throw new Exception();

			var groupId = await _context.Services_Groups.Where(sg => sg.ServiceId == id).Select(sg => sg.GroupId).FirstOrDefaultAsync();
                        

            return new ServiceVM() { Id = id, Name = service.Name, Price = service.Price, GroupId = groupId };
        }


		//public async Task<Service> GetServiceByIdWithGroup(int id) =>
  //          await _context.Services.Where(s => s.Id == id)
		//        .Include(s => s.Service_Group).ThenInclude(sg => sg.Group).FirstOrDefaultAsync();
	}
}
