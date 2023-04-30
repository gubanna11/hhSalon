using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Implementations;
using hhSalon.Services.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace hhSalonAPI.Hubs
{
	public class ChatHub: Hub
	{
		private readonly ChatService _chatService;
		private readonly IUsersService _usersService;
		public ChatHub(ChatService chatService, IUsersService usersService)
		{
			_chatService = chatService;
			_usersService = usersService;

		}

		public override async Task OnConnectedAsync()
		{
			//await Groups.AddToGroupAsync(Context.ConnectionId, "hhSalon");
			await Clients.Caller.SendAsync("UserConnected");
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			//await Groups.RemoveFromGroupAsync(Context.ConnectionId, "hhSalon");

			var user = _chatService.GetUserByConnectionId(Context.ConnectionId);
			_chatService.RemoveUserFromList(user);

			await base.OnDisconnectedAsync(exception);
		}


		public async Task AddUserConnectionId(string userId)
		{
			var user = _usersService.GetUserById(userId);
			_chatService.AddUserConnectionId(user, Context.ConnectionId);
		}



		public async Task CreatePrivateChat(Chat message)
		{
			string privateGroupName = GetGroupName(message.FromId, message.ToId);
			await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);

			var toUser = _usersService.GetUserById(message.ToId);
			var toConnectionId = _chatService.GetConnectionIdByUser(toUser);
			await Groups.AddToGroupAsync(toConnectionId, privateGroupName);

			//opening private chatbox for the other end user
			await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
		}


		public async Task ReceivePrivateMessage(Chat message)
		{
			//string privateGroupName = GetGroupName(message.FromId, message.ToId);
			//await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", message);
			var toUser = _usersService.GetUserById(message.ToId);
			var toConnectionId = _chatService.GetConnectionIdByUser(toUser);
			await Clients.Client(toConnectionId).SendAsync("NewPrivateMessage", message);
		}


		//public async Task RemovePrivateChat(string fromId, string toId)
		public async Task RemovePrivateChat(string id)
		{
			//string privateGroupName = GetGroupName(fromId, toId);
			//await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat");

			//await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);

			//var toUser = _usersService.GetUserById(toId);
			//var toConnectionId = _chatService.GetConnectionIdByUser(toUser);
			//await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);

			var user = _usersService.GetUserById(id);
			_chatService.RemoveUserFromList(user);
		}


		//private async Task UserChat(string fromId, string toId)
		//{
		//	var users = _chatService.GetUserMessagesList(fromId);

		//	var groupName = GetWorkerUserGroupName(fromId, toId);

		//	await Clients.Groups(groupName).SendAsync("UserChatList", users);
		//}



		private string GetGroupName(string firstId, string secondId)
		{
			var stringCompare = string.CompareOrdinal(firstId, secondId) < 0;
			return stringCompare ? $"{firstId}-{secondId}" : $"{secondId}-{firstId}"; 
		}
	}
}
