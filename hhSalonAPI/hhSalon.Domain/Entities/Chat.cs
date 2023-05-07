using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhSalon.Domain.Entities
{
	public class Chat
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Column("from_id")]
		public string FromId { get; set; }
		[ForeignKey(nameof(FromId))]
		public User FromUser { get; set; }

		[Column("to_id")]
		public string ToId { get; set; }
		[ForeignKey(nameof(ToId))]
		public User ToUser { get; set; }


		[Column("content")]
		public string Content { get; set; }

		[Column("date")]

		public DateTime Date { get; set; }


		[Column("is_read")]
		public bool IsRead { get; set; }
	}
}
