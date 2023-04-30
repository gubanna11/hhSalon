using hhSalon.Domain.Entities;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace hhSalon.Services.Services.Implementations
{
	public class ChatService
	{
		private static readonly Dictionary<User, string> Users = new Dictionary<User, string>();
	

		public bool AddUserToList(User userToAdd)
		{
			lock (Users)
			{
				foreach (var user in Users)
				{
					if (user.Key.Id == userToAdd.Id)
					{
						return false;
					}
				}

				Users.Add(userToAdd, null);
				return true;
			}
		}

		public void AddUserConnectionId(User user, string connectionId)
		{
			//AddUserToList(user);
			lock (Users)
			{
				//if (Users.ContainsKey(user))
				if(Users.Where(u => u.Key.Id == user.Id).Count() != 0)
				{
					//Users[user] = connectionId;
					var u = Users.Where(u => u.Key.Id == user.Id).FirstOrDefault();
					Users[u.Key] = connectionId;
				}
			}
		}

		public User GetUserByConnectionId(string connectionId)
		{
			lock (Users)
			{
				return Users.Where(x => x.Value == connectionId).Select(x => x.Key).FirstOrDefault();
			}
		}

		public string GetConnectionIdByUser(User user)
		{
			lock (Users)
			{
				return Users.Where(x => x.Key.Id == user.Id).Select(x => x.Value).FirstOrDefault();
			}
		}


		public void RemoveUserFromList(User user)
		{
			lock (Users)
			{
				if (user != null)
				{
					Users.Remove(Users.Where(x => x.Key.Id == user.Id).Select(x => x.Key).FirstOrDefault());
				}
				//if (Users.ContainsKey(user))
				//{
				//	Users.Remove(user);
				//}
			}
		}

		//public List<ChatItem> GetUserMessagesList(string userId)
		//{
		//	lock (Users)
		//	{
		//		return Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
				
		//	}
		//}
	}
}
