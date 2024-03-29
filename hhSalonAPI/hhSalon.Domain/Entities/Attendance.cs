﻿using hhSalon.Domain.Abstract;
using hhSalon.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hhSalon.Domain.Entities
{
	public class Attendance : IEntityBase
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Column("client_id"), Required]
		public string ClientId { get; set; }
		public User Client { get; set; }

		[Column("group_id"),]

		public int? GroupId { get; set; }
		public GroupOfServices Group { get; set; }

		[Column("service_id")]
		public int? ServiceId { get; set; }
		public Service Service { get; set; }


		[Column("worker_id"), Required]
		public string WorkerId { get; set; }
		public Worker Worker { get; set; }


		[Column("date"), Required]
		//[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
		public DateTime Date { get; set; }

		[Column("time")]
		//[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}", NullDisplayText = "")]
		public TimeSpan? Time { get; set; }


		[Column("price")]
		public double Price { get; set; }

		[Column("rendered"), StringLength(3)]
		public string IsRendered { get; set; }

		[Column("paid"),StringLength(3)]
		public string IsPaid { get; set; }

	}
}
