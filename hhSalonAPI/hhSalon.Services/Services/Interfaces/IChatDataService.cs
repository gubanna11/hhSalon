using hhSalon.Domain.Entities;
using hhSalon.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhSalon.Services.Services.Interfaces
{
	public interface IChatDataService
	{
		IEnumerable<ChatItem> GetUserMessagesList(string userId);
		Task<IEnumerable<Chat>> GetMessagesOfUser(string userId, string fromId);
		Task SaveMessage(Chat message);

		Task UpdateMessage(Chat message);
	}
}
