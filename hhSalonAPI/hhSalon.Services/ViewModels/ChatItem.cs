using hhSalon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhSalon.Services.ViewModels
{
	public class ChatItem
	{
		public string UserId { get; set; }
		public User User { get; set; }
		public int MessageUnreadCount { get; set; }
		public string LastMessage { get; set; }


		public string ToUserId { get; set; }
		public User ToUser { get; set; }
	}
}
