using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Implementations;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace hhSalonAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChatController : ControllerBase
	{
		private readonly ChatService _chatService;
		private readonly IUsersService _usersService;
		private readonly IChatDataService _chatDataService;

		public ChatController(ChatService chatService, IUsersService usersService, IChatDataService chatDataService)
		{
			_chatService = chatService;
			_usersService = usersService;
			_chatDataService = chatDataService;
		}

		[HttpPost("add-user/{userId}")]
		public ActionResult AddUser(string userId)
		{
			var user = _usersService.GetUserById(userId);

			if(user != null)
				if (_chatService.AddUserToList(user))
				{
					return Ok();
				}
			return BadRequest();
		}

		[HttpGet("chat/{userId}")]
		public ActionResult<List<ChatItem>> ChatList(string userId)
		{
			var user = _usersService.GetUserById(userId);

			if (user != null)
				return Ok(_chatDataService.GetUserMessagesList(userId).ToList());
			return BadRequest();
		}


		[HttpGet("chat/messages")]
		public async Task<ActionResult<List<Chat>>> MessagesOfUser(string user, string from)
		{
			var messages = await _chatDataService.GetMessagesOfUser(user, from);
			
			return Ok(messages.ToList());			
		}

		[HttpPost("save-message")]
		public async Task<ActionResult> SaveMessage(Chat message)
		{
			try
			{
				await _chatDataService.SaveMessage(message);
				return Ok();
			}
			catch(Exception ex)
			{
				return BadRequest(ex.InnerException.Message);
			}
			
		}

	}

	
}
