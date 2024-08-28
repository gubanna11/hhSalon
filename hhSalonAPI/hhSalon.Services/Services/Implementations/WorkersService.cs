using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Domain.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using hhSalon.Domain.Entities.Enums;

namespace hhSalon.Services.Services.Implementations
{
	public class WorkersService : IWorkersService
	{

		private readonly AppDbContext _context;
		public WorkersService(AppDbContext context)
		{
			_context = context;
		}


		public async Task<IEnumerable<WorkerVM>> GetWorkersByGroupId(int groupId)
		{
			var workers_groups = await _context.Workers_Groups.Where(w_g => w_g.GroupId == groupId).Select(wg => new WorkerVM
			{
				Id = wg.WorkerId,
				FirstName = wg.Worker.User.FirstName,
				LastName = wg.Worker.User.LastName,
				//UserName= wg.Worker.User.UserName,
				//GroupsNames = _context.Workers_Groups.Where(obj => obj.WorkerId == wg.WorkerId).Select(g => g.Group.Name).ToList()
			}).ToListAsync();


			return workers_groups;
		}

		//public async Task<IEnumerable<WorkerVM>> GetWorkersAsync()
		//{
		//	var workers = await _context.Workers_Groups.Select(wg => new WorkerVM
		//	{
		//		Id = wg.WorkerId,
		//		FirstName = wg.Worker.User.FirstName,
		//		LastName = wg.Worker.User.LastName,
		//		UserName= wg.Worker.User.UserName,
		//		GroupsNames = _context.Workers_Groups.Where(obj => obj.WorkerId == wg.WorkerId).Select(g => g.Group.Name).ToList()
		//	}).ToListAsync();


		//	return workers;
		//}

		public async Task<IEnumerable<Worker>> GetWorkersAsync()
		{
			return await _context.Workers.Include(w => w.User).Include(w => w.Schedules)
				.Include(w => w.Workers_Groups)
					.ThenInclude(wg => wg.Group).ToListAsync();
		}


		public async Task<Worker> GetWorkerByIdAsync(string workerId)
		{
			return await _context.Workers.Where(w => w.Id == workerId).Include(w => w.User).FirstOrDefaultAsync();
		}

		public async Task<WorkerVM> GetWorkerVMByIdAsync(string workerId)
		{
			WorkerVM workerVM = new WorkerVM();

			var worker = await _context.Workers.Where(w => w.Id == workerId).Include(w => w.User)
				.Include(w => w.Schedules)
				.Include(w => w.Workers_Groups).ThenInclude(wg => wg.Group).FirstOrDefaultAsync();

			workerVM.Id = workerId;
			workerVM.Address = worker.Address;
			workerVM.Email = worker.User.Email;
			workerVM.FirstName = worker.User.FirstName;
			workerVM.LastName = worker.User.LastName;
			workerVM.GroupsIds = worker.Workers_Groups.Select(wg => wg.GroupId).ToList();
			workerVM.Gender = worker.Gender;

			workerVM.UserName = worker.User.UserName;

			workerVM.Schedules = worker.Schedules;

			var days = Enum.GetValues(typeof(Days)).Cast<Days>().ToList();

			List<Schedule> schedules = new List<Schedule>();
			foreach (var day in days)
			{
				schedules.Add(worker.Schedules.Where(w => w.Day == day.ToString()).FirstOrDefault());
				if (schedules.Last() == null)
					schedules[schedules.Count - 1] = new Schedule
					{
						Day = day.ToString(),
						WorkerId = workerVM.Id,
						End = TimeSpan.Zero,
						Start = TimeSpan.Zero
					};
			}

			workerVM.Schedules = schedules;

			return workerVM;
		}


		public async Task CreateWorkerschedule(List<Schedule> schedules)
		{

			var workerSchedules = _context.Schedules.Where(s => s.WorkerId == schedules[0].WorkerId).ToList();


			if (workerSchedules.Count != 0)
			{
				_context.RemoveRange(workerSchedules);
				_context.SaveChanges();
			}

			schedules = schedules.Where(s => s.Start != TimeSpan.Zero && s.End != TimeSpan.Zero).ToList();

			_context.Schedules.AddRange(schedules);
			_context.SaveChanges();
		}


		private async Task UpdateWorkerInfo(WorkerVM workerVM)
		{
			var count =  _context.Users.Where(u => u.Email == workerVM.Email).Count();

			if (count > 1)
			{
				throw new Exception("This email is taken");
			}

			//var worker = await GetWorkerByIdAsync(workerVM.Id);
			var worker = await _context.Workers.Where(w => w.Id == workerVM.Id).Include(w => w.User).FirstOrDefaultAsync();
			//_context.Entry(worker).State = EntityState.Unchanged;
			if (worker is not null)
			{
				worker.Address = workerVM.Address;
				worker.Gender = workerVM.Gender;


				worker.User.FirstName = workerVM.FirstName;
				worker.User.LastName = workerVM.LastName;
				worker.User.Email = workerVM.Email;

				await _context.SaveChangesAsync();
				_context.Entry(worker.User).State = EntityState.Detached;
				_context.Entry(worker).State = EntityState.Detached;
			}

		}

		private async Task UpdateWorkersGroups(WorkerVM workerVM)
		{
			var worker_groups = _context.Workers_Groups.AsNoTracking().Where(wg => wg.WorkerId == workerVM.Id).ToList();

			_context.Workers_Groups.RemoveRange(worker_groups);
			await _context.SaveChangesAsync();

			worker_groups.ForEach(wg => { _context.Entry(wg).State = EntityState.Detached; });

			var list = new List<WorkerGroup>();
			foreach (var groupId in workerVM.GroupsIds)
			{
				WorkerGroup wg = new WorkerGroup
				{
					WorkerId = workerVM.Id,
					GroupId = groupId,
				};

				list.Add(wg);
			}

			await _context.Workers_Groups.AddRangeAsync(list);

			await _context.SaveChangesAsync();
			list.ForEach(wg => { _context.Entry(wg).State = EntityState.Detached; });
		}

		private async Task UpdateWorkerSchedule(WorkerVM workerVM)
		{

			//SCHEDULE
			var workerSchedules = await _context.Schedules.Where(s => s.WorkerId == workerVM.Id).ToListAsync();



			if (workerSchedules.Count != 0)
			{
				_context.Schedules.RemoveRange(workerSchedules);
				await _context.SaveChangesAsync();

			}


			var schedules = workerVM.Schedules.Where(s => s.Start != TimeSpan.Zero && s.End != TimeSpan.Zero).ToList();

			schedules.ForEach(w => _context.Entry(w).State = EntityState.Unchanged);

			if (schedules.Count > 0)
			{
				await _context.Schedules.AddRangeAsync(schedules);
				await _context.SaveChangesAsync();
			}

		}

		public async Task UpdateWorkerAsync(WorkerVM workerVM)
		{

			await UpdateWorkerInfo(workerVM);

			await UpdateWorkersGroups(workerVM);

			await UpdateWorkerSchedule(workerVM);


		}

		public async Task DeleteAsync(string workerId)
		{
			var worker = await _context.Users.Where(u => u.Id == workerId).FirstOrDefaultAsync();

			if (worker == null)
				throw new NullReferenceException("There is no worker with this user name!");

			_context.Users.Remove(worker);
			_context.SaveChanges();
		}

		//public async Task UpdateWorkerSchedule(List<Schedule> schedules)
		//{
		//	var workerSchedules = await _context.Schedule.Where(s => s.WorkerId == schedules[0].WorkerId).ToListAsync();


		//	if (workerSchedules.Count != 0)
		//	{
		//		_context.RemoveRange(workerSchedules);
		//		await _context.SaveChangesAsync();
		//	}

		//	await _context.AddRangeAsync(schedules);
		//	await _context.SaveChangesAsync();
		//}
	}
}

