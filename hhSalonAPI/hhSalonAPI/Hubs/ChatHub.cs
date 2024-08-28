using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Implementations;
using hhSalon.Services.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace hhSalonAPI.Hubs
{
    public class ChatHub : Hub
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
            await Clients.Caller.SendAsync("UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

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


            await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
        }

        public async Task ReceivePrivateMessage(Chat message)
        {
            var toUser = _usersService.GetUserById(message.ToId);
            var toConnectionId = _chatService.GetConnectionIdByUser(toUser);
            await Clients.Client(toConnectionId).SendAsync("NewPrivateMessage", message);
        }

        public async Task RemovePrivateChat(string id)
        {
            var user = _usersService.GetUserById(id);
            _chatService.RemoveUserFromList(user);
        }

        public async Task ReadPrivateMessage(Chat message)
        {
            var toUser = _usersService.GetUserById(message.FromId);
            var toConnectionId = _chatService.GetConnectionIdByUser(toUser);

            await Clients.Client(toConnectionId).SendAsync("MyMessageIsRead", message);
        }
        private string GetGroupName(string firstId, string secondId)
        {
            var stringCompare = string.CompareOrdinal(firstId, secondId) < 0;
            return stringCompare ? $"{firstId}-{secondId}" : $"{secondId}-{firstId}";
        }
    }
}
