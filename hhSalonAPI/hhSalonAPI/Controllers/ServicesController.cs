using hhSalon.Domain.Entities.Static;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hhSalonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }


        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetServicesVMsByGroupId(int groupId)
        {
            try
            {
                var services = await _servicesService.GetServicesByGroupIdAsync(groupId);

                //if(services is null)
                //                return NotFound(new { Message = "Empty!" });

                return Ok(services);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateService(ServiceVM newService)
        {
            try
            {
                await _servicesService.AddNewServiceAsync(newService);
                return Ok(await _servicesService.GetServicesByGroupIdAsync(newService.GroupId));
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Some errors!" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                var serviceVM = await _servicesService.GetServiceVMByIdAsync(id);

                await _servicesService.DeleteAsync(id);

                try
                {
                    var services = await _servicesService.GetServicesByGroupIdAsync(serviceVM.GroupId);

                    return Ok(services);
                }
                catch (Exception ex)
                {
                    return Ok(new { Message = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateService(ServiceVM serviceVM)
        {
            try
            {
                await _servicesService.UpdateServiceAsync(serviceVM);

                return Ok(await _servicesService.GetServicesByGroupIdAsync(serviceVM.GroupId));
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { Message = "This Service already exists!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
