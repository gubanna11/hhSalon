using hhSalon.Domain.Entities;
using hhSalon.Domain.Entities.Static;
using hhSalon.Services.Models.Dto;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Domain.Concrete;
using hhSalonAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace hhSalonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AuthController(AppDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
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

            //user.Token = CreateJwt(user);
            //await _context.SaveChangesAsync();

            //return Ok(new
            //{
            //	Token = user.Token,
            //	Message = "Login success!"
            //});

            user.Token = CreateJwt(user);
            var newAccessToken = user.Token;
            var newRefreshToken = CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);

            await _context.SaveChangesAsync();

            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj, string role = UserRoles.Client)
        {
            if (userObj == null)
            {
                return BadRequest();
            }

            userObj.Id = Guid.NewGuid().ToString();

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

            await _context.Users.AddAsync(userObj);
            await _context.SaveChangesAsync();

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
				//new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
				new Claim(ClaimTypes.Name, $"{user.UserName}"),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }


        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _context.Users
                .Any(u => u.RefreshToken == refreshToken);

            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }



        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("This is Invalid Token");
            }
            return principal;
        }



        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid Client Request");

            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);

            var userName = principal.Identity.Name;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid Request");


            var newAccessToken = CreateJwt(user);
            var newRefreshToken = CreateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _context.SaveChangesAsync();

            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }



        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCOde = 404,
                    Message = "Email doesn't exist"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);

            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);

            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));

            _emailService.SendEmail(emailModel);

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Email has sent!"
            });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);

            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User doesn't exist"
                });
            }

            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpiry;

            if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid Reset link"
                });
            }

            //check password
            var pass = CheckPasswordStrength(resetPasswordDto.NewPassword);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass });


            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Password reset successfully"
            });
        }



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
                Message = "Worker Registered!",
                WorkerId = workerVM.Id
            });
        }





    }
}
