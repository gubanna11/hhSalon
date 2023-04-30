using hhSalon.Domain.Entities;
using hhSalon.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhSalon.Services.Services.Interfaces
{
	public interface IWorkersService
	{

		Task<IEnumerable<Worker>> GetWorkersAsync();

		Task<IEnumerable<WorkerVM>> GetWorkersByGroupId(int groupId);


		Task CreateWorkerschedule(List<Schedule> schedules);


		Task<WorkerVM> GetWorkerVMByIdAsync(string workerId);
		Task<Worker> GetWorkerByIdAsync(string workerId);

		Task UpdateWorkerAsync(WorkerVM workerVM);

		Task DeleteAsync(string workerId);


		//Task UpdateWorkerInfo(WorkerVM workerVM);
		//Task UpdateWorkersGroups(WorkerVM workerVM);

		//Task UpdateWorkerSchedule(WorkerVM workerVM);

		//Task UpdateWorkerSchedule(List<Schedule> schedules);

	}
}
