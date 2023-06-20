using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hhSalonAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AttendancesController : ControllerBase
	{
		private readonly IAttendancesService _attendancesService;

		public AttendancesController(IAttendancesService attendancesService)
		{
			_attendancesService = attendancesService;
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
			catch(DbUpdateException ex)
			{
                return BadRequest(new { Message = ex.Message });
            }
			catch(Exception ex)
			{
				return BadRequest( new { Message = ex.Message});
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

        [HttpGet("worker-not-rendered-attendances/{workerId}")]
        public async Task<ActionResult> WorkerNotRendered(string workerId)
        {
            var attendances = await _attendancesService.WorkerNotRenderedAttendances(workerId);

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

            return Ok(new { Message = "Updated successfully!" });
        }

		[HttpPut]
		public async Task<ActionResult> UpdateAttendance(Attendance attendance)
		{
			await _attendancesService.UpdateAttendance(attendance);

            return Ok(new { Message = "Updated successfully!" });
        }


		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteAttendance(int id)
		{
			try
			{
				await _attendancesService.DeleteAsync(id);

				return Ok(new { Message = "Deleted successfully!" });
			}
			catch (Exception ex)
			{
				return NotFound(new { Message = "Not Found!" });
			}
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
