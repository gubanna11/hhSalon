using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace hhSalonAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GroupsController : ControllerBase
	{
		private readonly IGroupsService _groupsService;
		
		public GroupsController(IGroupsService groupsService)
		{
			_groupsService = groupsService;
		}

		[HttpGet]
		public async Task<ActionResult<List<GroupOfServices>>> GetGroups() 
		{
			return Ok(await _groupsService.GetAllAsync());
		}

		
		[HttpPost]
		public async Task<ActionResult<List<GroupOfServices>>> CreateGroup(GroupOfServices newGroup)
		{

			await _groupsService.AddAsync(newGroup);

			return Ok(await _groupsService.GetAllAsync());
		}

		private async void UploadFIle()
		{
			var formCollection = await Request.ReadFormAsync();
			var file = formCollection.Files.First();
			

			var folderName = Path.Combine("Resources", "Images");

			var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

			if (file.Length > 0)
			{
				var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

				var fullPath = Path.Combine(pathToSave, fileName);

				var dbPath = Path.Combine(folderName, fileName);

				using (var stream = new FileStream(fullPath, FileMode.Create))
				{
					file.CopyTo(stream);
				}
			}



			//string uniqueFileName = null;

			//if (file != null)
			//{
			//	string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/groups");
			//	if (System.IO.File.Exists(uploadsFolder + "/" + file.FileName))
			//		return file.FileName;

			//	uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
			//	string filePath = Path.Combine(uploadsFolder, uniqueFileName);
			//	using (var fileStream = new FileStream(filePath, FileMode.Create))
			//	{
			//		file.CopyTo(fileStream);
			//	}
			//}

			//return uniqueFileName;
		}


		[HttpGet("{groupId}")]
		public async Task<ActionResult<GroupOfServices>> GetGroupById(int groupId)
		{
			var group = await _groupsService.GetByIdAsync(groupId);


			if (group == null)
				return BadRequest("Group not found");

			return Ok(group);
		}

		[HttpPut]
		public async Task<ActionResult<List<GroupOfServices>>> UpdateGroup(GroupOfServices group)
		{
			await _groupsService.UpdateGroupAsync(group);

			return Ok(await _groupsService.GetAllAsync());
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<List<GroupOfServices>>> DeleteGroupById(int id)
		{
			var group = await _groupsService.GetByIdAsync(id);
			if (group == null)
				return BadRequest("Group not found");

			await _groupsService.DeleteAsync(id);

			return Ok(await _groupsService.GetAllAsync());
		}


		[HttpGet("worker/{workerId}")]
		public async Task<ActionResult<GroupOfServices>> GetGroupsByWorkerId(string workerId)
		{
			var groups = await _groupsService.GetGroupsByWorkerId(workerId);


			if (groups == null)
				return BadRequest("Groups weren't found");

			return Ok(groups);
		}
	}
}
