using hhSalon.Domain.Entities;
using hhSalon.Domain.Entities.Static;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace hhSalonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly IWorkersService _workersService;
        public WorkersController(IWorkersService workersService)
        {
            _workersService = workersService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Worker>> GetWorkers()
        {
            return Ok(await _workersService.GetWorkersAsync());
        }

        [HttpGet("{groupId}")]
        public async Task<ActionResult<WorkerVM>> GetWorkersByGroupId(int groupId)
        {
            return Ok(await _workersService.GetWorkersByGroupId(groupId));
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("info")]
        public async Task<ActionResult<WorkerVM>> GetWorkerById(string workerId)
        {
            return Ok(await _workersService.GetWorkerVMByIdAsync(workerId));
        }

        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Worker}")]
        [HttpPost("schedule/create")]
        public async Task<ActionResult> CreateWorkerSchedule([FromBody] List<Schedule> schedules)
        {

            if (schedules.Count == 0 || (schedules.Where(s => (s.Start != TimeSpan.Zero && s.End == TimeSpan.Zero)
                || (s.Start == TimeSpan.Zero && s.End != TimeSpan.Zero)).Count() > 0))
                return BadRequest(new
                {
                    Message = "Worker's schedule should have start and end time!",
                });


            await _workersService.CreateWorkerschedule(schedules);

            return Ok(new
            {
                Message = "Worker's schedule was updated!",
            });
        }

        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Worker}")]
        [HttpPut]
        public async Task<ActionResult> UpdateWorker(WorkerVM workerVM)
        {
            try
            {
                if (workerVM.Schedules.Where(s => (s.Start != TimeSpan.Zero && s.End == TimeSpan.Zero)
                || (s.Start == TimeSpan.Zero && s.End != TimeSpan.Zero)).Count() > 0)
                    return BadRequest(new
                    {
                        Message = "Worker's schedule should have start and end time!",
                    });

                await _workersService.UpdateWorkerAsync(workerVM);
            }

            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                });
            }

            return Ok(new
            {
                Message = "Worker's data was updated!",
            });
        }


        //[Authorize(Roles = UserRoles.Admin)]
        //[HttpPut("schedule/update")]
        //public async Task<ActionResult> UpdateWorkerSchedule([FromBody] List<Schedule> schedules)
        //{

        //	if (schedules.Count == 0)
        //		return BadRequest(new
        //		{
        //			Message = "Worker Schedule should have start and end time!",
        //		});

        //	await _workersService.UpdateWorkerSchedule(schedules);

        //	return Ok(new
        //	{
        //		Message = "Worker Schedule Updated!",
        //	});
        //}


        [HttpDelete]
        public async Task<ActionResult<List<WorkerVM>>> Delete(string workerId)
        {
            try
            {
                await _workersService.DeleteAsync(workerId);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }

            return Ok(await _workersService.GetWorkersAsync());
        }
    }
}
