using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace hhSalon.Services.ViewModels
{
	public class ServiceVM
	{
		public int Id { get; set; }

		[Required]
		[StringLength(45)]
		public string Name { get; set; }

		[Required]
		public double Price { get; set; }

		public int GroupId { get; set; }
	}
}
