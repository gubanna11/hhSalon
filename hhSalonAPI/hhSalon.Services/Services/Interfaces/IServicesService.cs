using hhSalon.Domain.Abstract.Interfaces;
using hhSalon.Domain.Entities;
using hhSalon.Services.ViewModels;

namespace hhSalon.Services.Services.Interfaces
{
    public interface IServicesService
    {        
        Task<List<ServiceVM>> GetServicesByGroupIdAsync(int groupId);
        Task AddNewServiceAsync(ServiceVM newService);
        Task UpdateServiceAsync(ServiceVM serviceVM);
        Task<ServiceVM> GetServiceVMByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
