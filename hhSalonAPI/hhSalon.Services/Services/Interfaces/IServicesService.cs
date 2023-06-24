using hhSalon.Domain.Abstract;
using hhSalon.Domain.Entities;
using hhSalon.Services.ViewModels;

namespace hhSalon.Services.Services.Interfaces
{
    public interface IServicesService : IEntityBaseRepository<Service>
    {
        Task<ServiceVM> GetServiceVMByIdAsync(int id);
        Task<List<ServiceVM>> GetServicesByGroupIdAsync(int groupId);
        Task AddNewServiceAsync(ServiceVM newService);
        Task UpdateServiceAsync(ServiceVM serviceVM);

	}
}
