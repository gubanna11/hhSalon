//using hhSalon.Domain.Entities.Enums;
using hhSalon.Domain.Entities.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhSalon.Domain.Entities
{
	public class Worker
	{
		[Column("id")]
		public string Id { get; set; }
		[ForeignKey("Id")]
		public User User { get; set; }


		[Column("address")]
		[StringLength(45)]
		public string Address { get; set; }

		[Column("gender"), StringLength(6)]
		public string Gender { get; set; }

		public List<WorkerGroup> Workers_Groups { get; set; }
		public List<Schedule> Schedules { get; set; }
		public List<Attendance> Attendances { get; set; }
	}
}
