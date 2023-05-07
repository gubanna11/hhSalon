using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
			var list1 = _context.Chats.Where(c => c.FromId == userId).Include(c => c.ToUser).Include(c => c.FromUser).ToList();
			var list2 = _context.Chats.Where(c => c.ToId == userId).Include(c => c.ToUser).Include(c => c.FromUser).ToList();
			var list = new List<Chat>();


			list.AddRange(list1);
			list.AddRange(list2);

			var resultList = new List<Chat>();

			list.ForEach(
				l =>
				{
					if(resultList.Where(item => (item.FromId == l.FromId && item.ToId == l.ToId) || (item.FromId == l.ToId && item.ToId == l.FromId)).Count() == 0)
					{
						var sortedList = list.Where(item => (item.FromId == l.FromId && item.ToId == l.ToId) || (item.FromId == l.ToId && item.ToId == l.FromId))
							.OrderBy(item => item.Date).ToList();
						var temp = sortedList.LastOrDefault();
						if (!resultList.Contains(temp))
							resultList.Add(temp);
					}					
				});

			

			var result = resultList.Select(l => new ChatItem
			{
				UserId = l.FromId,
				User = l.FromUser,
				ToUserId = l.ToId,
				ToUser = l.ToUser,
				MessageUnreadCount = _context.Chats.Where(r => ((r.FromId == l.FromId && r.ToId == l.ToId))
					&& !r.IsRead).Count(),
				LastMessage = l.Content,
				IsRead = l.IsRead,
				Date = l.Date
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
			message.FromUser = null;
			message.ToUser = null;
			//Chat chat = new Chat
			//{
			//	FromId = message.FromId,
			//	ToId = message.ToId,
			//	Date = DateTime.Now,
			//	Content = message.Content
			//};
			await _context.Chats.AddAsync(message);
			 await _context.SaveChangesAsync();
		}

		public async Task UpdateMessage(Chat message)
		{
			//var chat = await _context.Chats.Where(c => c.FromId == message.FromId && c.ToId == message.ToId && c.Content == message.Content && c.Date == message.Date).FirstOrDefaultAsync();
			//var chat = await _context.Chats.AsNoTracking().Where(c => c.Id == message.Id).FirstOrDefaultAsync();
			//chat.IsRead = true;
			message.IsRead = true;
			_context.Chats.Update(message);
			await _context.SaveChangesAsync();
		}
	}
}
