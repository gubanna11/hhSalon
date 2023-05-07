using hhSalon.Domain.Entities.Static;
using hhSalon.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using hhSalonAPI.Domain.Concrete;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.Services.Implementations;
using hhSalon.Services.ViewModels;

namespace hhSalonAPI.Controllers
{
	[Route("api/[controller]")]
	//[Route("api/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		
		private readonly AppDbContext _context;
		private readonly IUsersService _usersService;
		public UsersController(AppDbContext context, IUsersService usersService)
		{
			_context = context;
			_usersService = usersService;
		}

		[Authorize(Roles = UserRoles.Admin)]
		[HttpGet]
		public async Task<ActionResult<User>> GetAllUser()
		{
			return Ok(await _usersService.GetAllUser());
		}


		
		[HttpGet("{userId}")]
		public ActionResult<User> GetUserById(string userId)
		{
			return Ok(_usersService.GetUserById(userId));
		}

		

	}
}
