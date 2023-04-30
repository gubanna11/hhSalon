using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Domain.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace hhSalonAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServicesController : ControllerBase
	{
		private readonly IServicesService _servicesService;
		private readonly IGroupsService _groupsService;

		public ServicesController(IServicesService servicesService, AppDbContext context, IGroupsService groupsService)
		{
			_servicesService = servicesService;
			_groupsService = groupsService;
		}


		[HttpGet("{groupId}")]
		public async Task<ActionResult<List<ServiceVM>>> GetServicesByGroupId(int groupId)
		{
			return Ok(await _servicesService.GetServicesByGroupIdAsync(groupId));
		}

		[HttpPost]
		public async Task<ActionResult<List<ServiceVM>>> CreateService(ServiceVM newService)
		{
			try
			{
				await _servicesService.AddNewServiceAsync(newService);
			}
			catch (DbUpdateException)
			{
				return BadRequest(new { Message = "This Service already exists!" });
			}

			return Ok(await _servicesService.GetServicesByGroupIdAsync(newService.GroupId));
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<List<ServiceVM>>> DeleteService(int id)
		{
			var serviceVM = await _servicesService.GetServiceByIdWithGroupAsync(id);
			
			await _servicesService.DeleteAsync(id);

			return Ok(await _servicesService.GetServicesByGroupIdAsync(serviceVM.GroupId));
		}

		[HttpPut]
		public async Task<ActionResult<List<ServiceVM>>> UpdateService(ServiceVM serviceVM)
		{
			try
			{
				await _servicesService.UpdateServiceAsync(serviceVM);

				return Ok(await _servicesService.GetServicesByGroupIdAsync(serviceVM.GroupId));
			}
			catch (DbUpdateException)
			{
				return BadRequest(new { Message = "This Service already exist!" });
			}			

		}
	}
}
