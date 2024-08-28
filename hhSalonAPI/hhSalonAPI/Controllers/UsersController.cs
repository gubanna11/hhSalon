using hhSalon.Domain.Entities;
using hhSalon.Domain.Entities.Static;
using hhSalon.Services.Services.Interfaces;
using hhSalonAPI.Domain.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hhSalonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
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

        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser(User user)
        {
            await _usersService.UpdateUser(user);

            return Ok(new
            {
                Message = "User's data was updated!",
            });
        }
    }
}
