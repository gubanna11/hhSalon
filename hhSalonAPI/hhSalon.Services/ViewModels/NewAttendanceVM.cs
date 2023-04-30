using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using hhSalon.Domain.Entities.Enums;

namespace hhSalon.Services.ViewModels
{
	public class NewAttendanceVM
	{
		public string ClientId { get; set; }
		public int GroupId { get; set; }
		public int ServiceId { get; set; }

		public string WorkerId { get; set; }

		public DateTime Date { get; set; }
		public TimeSpan Time { get; set; }

		public double Price { get; set; }

		public string IsRendered { get; set; }

		public string IsPaid { get; set; }
	}
}
