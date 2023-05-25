using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hhSalonAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AttendancesController : ControllerBase
	{
		private readonly IAttendancesService _attendancesService;
		private readonly IUsersService _usersService;

		public AttendancesController(IAttendancesService attendancesService, IUsersService usersService)
		{
			_attendancesService = attendancesService;
			_usersService = usersService;
		}

		
		[HttpPost]
		[Authorize]
		public async Task<ActionResult> NewAttendance(NewAttendanceVM newAttendanceVM)
		{
			try
			{
				await _attendancesService.AddNewAttendanceAsync(newAttendanceVM);
				return Ok();
			}
			catch(Exception ex)
			{
				if (ex.InnerException.Message.Contains("Duplicate"))
					return BadRequest( new { Message = "You already have the appointment with the same service on this date"});
				return BadRequest();
			}			
		}

		[HttpGet("my-not-rendered-not-paid-attendances/{userId}")]
		[Authorize]
		public async Task<ActionResult> MyNotRenderedNotPaidAttendances(string userId)
		{
			var attendances = await _attendancesService.MyNotRenderedNotPaidAttendances(userId);

			return Ok(attendances);
		}

		[HttpGet("my-not-rendered-is-paid-attendances/{userId}")]
		[Authorize]
		public async Task<ActionResult> MyNotRenderedIsPaidAttendances(string userId)
		{
			var attendances = await _attendancesService.MyNotRenderedIsPaidAttendances(userId);

			return Ok(attendances);
		}

		[HttpGet("my-history/{userId}")]
		[Authorize]
		public async Task<ActionResult> MyHistory(string userId)
		{
			var attendances = await _attendancesService.MyIsRenderedAttendances(userId);

			return Ok(attendances);
		}


		//WORKERS DATA

		[HttpGet("worker-history/{workerId}")]

		public async Task<ActionResult> WorkerHistory(string workerId)
		{
			var attendances = await _attendancesService.WorkerIsRenderedAttendances(workerId);

			return Ok(attendances);
		}

		[HttpGet("worker-not-rendered-not-paid-attendances/{workerId}")]
		public async Task<ActionResult> WorkerNotRenderedNotPaid(string workerId)
		{
			var attendances = await _attendancesService.WorkerNotRenderedNotPaidAttendances(workerId);

			return Ok(attendances);
		}

		
		[HttpGet("worker-not-rendered-is-paid-attendances/{workerId}")]
		public async Task<ActionResult> WorkerNotRenderedIsPaid(string workerId)
		{
			var attendances = await _attendancesService.WorkerNotRenderedIsPaidAttendances(workerId);

			return Ok(attendances);
		}

		[HttpGet("time-slots/{workerId}/{day}")]
		public async Task<ActionResult> GetFreeTimeSlots(string workerId, DateTime day)
		{
			var slots = await _attendancesService.GetFreeTimeSlots(workerId, day);

			return Ok(slots);
		}


		[HttpPut("update-attendances")]
		public async Task<ActionResult> UpdateAttendances(List<Attendance> attendances)
		{
			await _attendancesService.UpdateAttendances(attendances);

			return Ok();
		}

		[HttpPut]
		public async Task<ActionResult> UpdateAttendance(Attendance attendance)
		{
			await _attendancesService.UpdateAttendance(attendance);

			return Ok();
		}


		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteAttendance(int id)
		{
			await _attendancesService.DeleteAsync(id);

			return Ok();
		}




		[HttpGet("all-attendances")]
		public async Task<ActionResult> GetAllAttendances()
		{
			var attendances = await _attendancesService.GeAllAttendances();

			return Ok(attendances);
		}

		[HttpGet]
		public async Task<ActionResult> FilterAttendances([FromQuery]string content)
		{
			IEnumerable<Attendance> attendances = new List<Attendance>();

			if (content == null)
				attendances = await _attendancesService.GeAllAttendances();
			
			else 
				attendances = await _attendancesService.GeAttendancesBySearch(content);

			return Ok(attendances);
		}
	}
}
