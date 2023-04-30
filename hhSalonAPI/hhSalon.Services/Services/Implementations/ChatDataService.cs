using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhSalon.Services.Services.Implementations
{
	public class ChatDataService : IChatDataService
	{
		private readonly AppDbContext _context;
		public ChatDataService(AppDbContext context)
		{
			_context = context;
		}


		public IEnumerable<ChatItem> GetUserMessagesList(string userId)
		{
			//var list = _context.Chats.Where(c => c.ToId == userId || c.FromId == userId).Include(c => c.FromUser).Include(c => c.ToUser)
			//	.ToList();
			//var chat = list.Select(c => new ChatItem
			//{
			//	UserId = c.FromId,
			//	User = c.FromUser,
			//	//MessageCount = _context.Chats.Where(h => h.From == c.From && h.To == userId && unreaded).Count()
			//	MessageUnreadCount = 0,
			//	LastMessage = _context.Chats.Where(h => h.FromId == c.FromId || h.ToId == userId).OrderBy(c => c.Date).LastOrDefault().Content,
			//	ToUserId = c.ToId,
			//	ToUser = c.ToUser
			//}).ToList();
			//return chat;


			var list1 = _context.Chats.Where(c => c.FromId == userId).Include(c => c.ToUser).Include(c => c.FromUser).ToList();
			var list2 = _context.Chats.Where(c => c.ToId == userId).Include(c => c.ToUser).Include(c => c.FromUser).ToList();
			var list = new List<Chat>();

			//if(list1.Count > list2.Count)
			list1.ForEach(
				l1 =>
				{
					var list1Ordered = list1.Where(l => l.ToId == l1.ToId).OrderBy(c => c.Date).ToList();

					var chat1 = list1Ordered.LastOrDefault();

					var chat2 = list2.Where(l2 => l1.ToId == l2.FromId).OrderBy(l2 => l2.Date).LastOrDefault();

					if (chat2 == null)
					{
						//if (list.Where(l => l.FromId == chat1.FromId && l.ToId == chat1.ToId).Count() == 0)
						if (!list.Contains(chat1))
							list.Add(chat1);
					}
					else if (chat2.Date < chat1.Date)
					{
						//if (list.Where(l => l.FromId == chat1.FromId && l.ToId == chat1.ToId).Count() == 0)
						if (!list.Contains(chat1))
							list.Add(chat1);
					}
					else
					{
						if (!list.Contains(chat2))
							list.Add(chat2);
					}
				});
			//else

			list2.ForEach(
				l2 =>
				{
					//if(!list.Contains(l2))
					//	list.Add(l2);
					var list2Ordered = list2.Where(l => l.FromId == l2.FromId).OrderBy(c => c.Date).ToList();

					var chat2 = list2Ordered.LastOrDefault();

					var chat1 = list1.Where(l => l2.FromId == l.ToId).OrderBy(l2 => l2.Date).LastOrDefault();

					if (chat1 == null)
					{
						//if (list.Where(l => l.FromId == chat2.FromId && l.ToId == chat2.ToId).Count() == 0)
						if (!list.Contains(chat2))
							list.Add(chat2);
					}
					else if (chat1.Date < chat2.Date)
					{
						//if (list.Where(l => l.FromId == chat2.FromId && l.ToId == chat2.ToId).Count() == 0)
						if (!list.Contains(chat2)) ;
						list.Add(chat2);
					}
					else
					{
						if (!list.Contains(chat1))
							list.Add(chat1);
					}
				});


			var result = list.Select(l => new ChatItem
			{
				UserId = l.FromId,
				User = l.FromUser,
				ToUserId = l.ToId,
				ToUser = l.ToUser,
				MessageUnreadCount = 0,
				LastMessage = l.Content
			});

			return result;
		}

		public async Task<IEnumerable<Chat>> GetMessagesOfUser(string userId, string fromId)
		{
			var messages = await _context.Chats.Where(c => (c.FromId == fromId && c.ToId == userId) || (c.FromId == userId && c.ToId == fromId))
				.Include(c => c.FromUser).Include(c => c.ToUser).OrderBy(c => c.Date).ToListAsync();
			return messages;

		}

		public async Task SaveMessage(Chat message)
		{
			Chat chat = new Chat
			{
				FromId = message.FromId,
				ToId = message.ToId,
				Date = DateTime.Now,
				Content = message.Content
			};
			await _context.Chats.AddAsync(chat);
			await _context.SaveChangesAsync();
		}
	}
}
