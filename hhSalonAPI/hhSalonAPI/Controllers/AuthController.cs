using hhSalonAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using hhSalonAPI.Domain.Concrete;
using hhSalon.Domain.Entities;
using hhSalon.Domain.Entities.Static;
using hhSalon.Services.ViewModels;

namespace hhSalonAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IConfiguration _configuration;
		public AuthController(AppDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		[HttpPost("authenticate")]
		public async Task<IActionResult> Authenticate([FromBody] User userObj)
		{
			if (userObj == null)
			{
				return BadRequest();
			}

			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.UserName == userObj.UserName);

			if (user == null)
				return NotFound(new { Message = "User not found!" });

			if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
				return BadRequest(new { Message = "Incorrect password!" });


			user.Token = CreateJwt(user);

			//await _context.SaveChangesAsync();

			return Ok(new
			{
				Token = user.Token,
				Message = "Login success!"
			});
		}


		[HttpPost("register")]
		public async Task<IActionResult> RegisterUser([FromBody] User userObj, string role = UserRoles.Client)
		{
			if (userObj == null)
			{
				return BadRequest();
			}


			//double id
			if (_context.Users.Where(u => u.Id == userObj.Id).Count() > 0)
				while (_context.Users.Where(u => u.Id == userObj.Id).Count() != 0)
					userObj.Id = Guid.NewGuid().ToString();


			//double name
			if (await CheckUserNameExistAsync(userObj.UserName))
				return BadRequest(new { Message = "This UserName is already taken!" });

			//double email

			if (await CheckEmailExistAsync(userObj.Email))
				return BadRequest(new { Message = "This Email is already taken!" });

			//check password

			var pass = CheckPasswordStrength(userObj.Password);
			if (!string.IsNullOrEmpty(pass))
				return BadRequest(new { Message = pass });


			userObj.Password = PasswordHasher.HashPassword(userObj.Password);
			userObj.Role = role;

			userObj.Token = "";

			userObj.Id = Guid.NewGuid().ToString();


			await _context.Users.AddAsync(userObj);
			await _context.SaveChangesAsync();


			//return Ok(_context.Users.ToList());
			return Ok(new
			{
				Message = "User Registered!"
			});
		}

		private async Task<bool> CheckUserNameExistAsync(string userName)
			=> await _context.Users.AnyAsync(u => u.UserName == userName);


		private async Task<bool> CheckEmailExistAsync(string email)
			=> await _context.Users.AnyAsync(u => u.Email == email);

		private string CheckPasswordStrength(string password)
		{
			StringBuilder stringBuilder = new StringBuilder();

			if (password.Length < 8)
				stringBuilder.Append("Minimum password length should be 8" + Environment.NewLine);

			if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
				&& Regex.IsMatch(password, "[0-9]")))
				stringBuilder.Append("Password should be Alphanumeric" + Environment.NewLine);

			if (!(Regex.IsMatch(password, @"[<,>@!#$%^&*()_+\[\]{}?:;|'./~\-=]")))
				stringBuilder.Append("Password should contain special characters" + Environment.NewLine);

			return stringBuilder.ToString();
		}


		private string CreateJwt(User user)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
			var identity = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Role, user.Role),
				new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
			});

			var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = identity,
				Expires = DateTime.Now.AddDays(1),
				//Expires = DateTime.Now.AddSeconds(7),
				SigningCredentials = credentials
			};

			var token = jwtTokenHandler.CreateToken(tokenDescriptor);

			return jwtTokenHandler.WriteToken(token);
		}


		//[Authorize (Roles = UserRoles.Admin)]
		//[HttpGet]
		//public async Task<ActionResult<User>> GetAllUser()
		//{
		//	return Ok(await _context.Users.ToListAsync());
		//}




		[HttpPost("worker-register")]
		public async Task<IActionResult> RegisterWorker([FromBody] WorkerVM workerVM)
		{
			if (workerVM == null)
			{
				return BadRequest();
			}
			workerVM.Id = Guid.NewGuid().ToString();

			//double id
			if (_context.Users.Where(u => u.Id == workerVM.Id).Count() > 0)
				while (_context.Users.Where(u => u.Id == workerVM.Id).Count() != 0)
					workerVM.Id = Guid.NewGuid().ToString();


			//double name
			if (await CheckUserNameExistAsync(workerVM.UserName))
				return BadRequest(new { Message = "This UserName is already taken!" });

			//double email

			if (await CheckEmailExistAsync(workerVM.Email))
				return BadRequest(new { Message = "This Email is already taken!" });

			//check password

			var pass = CheckPasswordStrength(workerVM.Password);
			if (!string.IsNullOrEmpty(pass))
				return BadRequest(new { Message = pass });


			workerVM.Password = PasswordHasher.HashPassword(workerVM.Password);
			workerVM.Role = UserRoles.Worker;

			workerVM.Token = "";

			

			User user = new User
			{
				Id = workerVM.Id,
				FirstName = workerVM.FirstName,
				LastName = workerVM.LastName,
				UserName = workerVM.UserName,
				Email = workerVM.Email,
				Password = workerVM.Password,
				Role = workerVM.Role
			};

			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();


			//add in workers table

			Worker worker = new Worker
			{
				Id = workerVM.Id,
				Address = workerVM.Address,
				Gender = workerVM.Gender,
			};

			await _context.Workers.AddAsync(worker);
			await _context.SaveChangesAsync();

			//add int workers_groups table

			var list = new List<WorkerGroup>();
			foreach (var groupId in workerVM.GroupsIds)
			{
				WorkerGroup worker_group = new WorkerGroup
				{
					WorkerId = workerVM.Id,
					GroupId = groupId
				};

				list.Add(worker_group);
			}

			await _context.Workers_Groups.AddRangeAsync(list);
			
			await _context.SaveChangesAsync();

			
			return Ok(new
			{
				Message = "Worker Registered!", WorkerId = workerVM.Id
			});
		}

	}
}
