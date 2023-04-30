using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using hhSalon.Domain.Entities.Enums;
using hhSalon.Domain.Entities;

namespace hhSalon.Services.ViewModels
{
	public class WorkerVM
	{
		public string? Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public string Token { get; set; }

		public string Role { get; set; }


		[StringLength(45)]
		public string Address { get; set; }

		[StringLength(6)]
		public string Gender { get; set; }


		public List<int> GroupsIds { get; set; }
		public List<Schedule> Schedules { get; set; }
	}
}
