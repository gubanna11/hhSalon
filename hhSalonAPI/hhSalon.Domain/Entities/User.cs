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


	}
}
