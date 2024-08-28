using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hhSalon.Domain.Entities
{
	public class User
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }
		[Column("first_name")]
		public string FirstName { get; set; }
		[Column("last_name")]
		public string LastName { get; set; }
		[Column("email")]
		public string Email { get; set; }
		[Column("user_name")]
		public string UserName { get; set; }
		[Column("password")]
		public string Password { get; set; }
		[Column("token")]
		public string Token { get; set; }
		[Column("role")]
		public string Role { get; set; }

		[Column("refresh_token")]
		public string RefreshToken { get; set; }
		[Column("refresh_token_exp_time")]
		public DateTime RefreshTokenExpiryTime { get; set; }

		[Column("reset_password_token")]
		public string ResetPasswordToken { get; set; }
        [Column("reset_password_expiry")]
        public DateTime ResetPasswordExpiry { get; set; }

		public List<Attendance> Attendances { get; set; }
	}
}
