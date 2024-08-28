using hhSalon.Domain.Abstract.Interfaces;
using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace hhSalon.Services.Services.Implementations
{
    public class ServicesService : IServicesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ServiceVM>> GetServicesByGroupIdAsync(int groupId)
        {
            var services = await _unitOfWork
                .GenericRepository<Service>()
                .Set
                .Include(s => s.ServiceGroup)
                .Where(s => s.ServiceGroup.GroupId == groupId)
                .ToListAsync();

            if (services is null || services.Count == 0)
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
            int count = _unitOfWork.GenericRepository<Service>().Set.Where(s => s.Name == newService.Name).Count();

            if (count > 0)
            {
                throw new DbUpdateException("This Service already exists!");
            }

            Service service = new Service
            {
                Name = newService.Name,
                Price = newService.Price
            };

            await _unitOfWork.GenericRepository<Service>().AddAsync(service);
            await _unitOfWork.SaveChangesAsync();

            ServiceGroup serviceGroup = new ServiceGroup
            {
                ServiceId = service.Id,
                GroupId = newService.GroupId
            };

            await _unitOfWork.GenericRepository<ServiceGroup>().AddAsync(serviceGroup);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateServiceAsync(ServiceVM serviceVM)
        {
            //var service = await GetByIdAsync(serviceVM.Id);
            var service = _unitOfWork
                .GenericRepository<Service>()
                .Set
                .Where(s => s.Id == serviceVM.Id)
                .Include(s => s.ServiceGroup)
                .FirstOrDefault();

            if (service != null)
            {

                service.Name = serviceVM.Name;
                service.Price = serviceVM.Price;

                await _unitOfWork.SaveChangesAsync();
                if (service.ServiceGroup.GroupId != serviceVM.GroupId)
                {
                    ServiceGroup serviceGroup = _unitOfWork
                        .GenericRepository<ServiceGroup>()
                        .Set
                        .Where(sg => sg.ServiceId == service.Id)
                        .FirstOrDefault();

                     _unitOfWork.GenericRepository<ServiceGroup>().DeleteAsync(serviceGroup.Id);

                    await _unitOfWork.SaveChangesAsync();

                    ServiceGroup newServiceGroup = new ServiceGroup()
                      {
                          ServiceId = service.Id,
                          GroupId = serviceVM.GroupId
                      };

                      await _unitOfWork.GenericRepository<ServiceGroup>().AddAsync(newServiceGroup);

                }

                await _unitOfWork.SaveChangesAsync();
            }
            else
                throw new NullReferenceException();
        }

        public async Task<ServiceVM> GetServiceVMByIdAsync(int id)
        {
            //var service = await _context.Services.Where(s => s.Id == id)
            //	.Include(s => s.Service_Group).ThenInclude(sg => sg.Group).FirstOrDefaultAsync();

            var service = await _unitOfWork
                .GenericRepository<Service>()
                .GetByIdAsync(id);

            if (service == null)
                throw new Exception();

            var groupId = await _unitOfWork
                .GenericRepository<ServiceGroup>()
                .Set
                .Where(sg => sg.ServiceId == id)
                .Select(sg => sg.GroupId)
                .FirstOrDefaultAsync();


            return new ServiceVM() { Id = id, Name = service.Name, Price = service.Price, GroupId = groupId };
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GenericRepository<Service>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }


        //public async Task<Service> GetServiceByIdWithGroup(int id) =>
        //          await _context.Services.Where(s => s.Id == id)
        //        .Include(s => s.Service_Group).ThenInclude(sg => sg.Group).FirstOrDefaultAsync();
    }
}
