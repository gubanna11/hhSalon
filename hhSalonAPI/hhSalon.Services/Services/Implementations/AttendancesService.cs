using hhSalon.Domain.Abstract;
using hhSalon.Domain.Entities.Static;
using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Interfaces;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace hhSalon.Services.Services.Implementations
{
    public class AttendancesService : EntityBaseRepository<Attendance>, IAttendancesService
	{
		private readonly AppDbContext _context;
		public AttendancesService(AppDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task AddNewAttendanceAsync(NewAttendanceVM newAttendance)
		{
			Attendance attendance = new Attendance()
			{
				GroupId = newAttendance.GroupId,
				ServiceId = newAttendance.ServiceId,
				WorkerId = newAttendance.WorkerId,
				Date = newAttendance.Date,
				Price = _context.Services.Where(s => s.Id == newAttendance.ServiceId).Select(s => s.Price).FirstOrDefault(),
				//IsRendered = YesNoEnum.No,
				//IsPaid = YesNoEnum.No,
				IsRendered = YesNo.No,
				IsPaid = YesNo.No,
				ClientId = newAttendance.ClientId,
				Time = newAttendance.Time
			};


			await _context.Attendances.AddAsync(attendance);
			await _context.SaveChangesAsync();
		}


		// USER NOT RENDERED + IS PAID

		public async Task<IEnumerable<Attendance>> MyNotRenderedIsPaidAttendances(string userId)
		{
			var attendances = await _context.Attendances.Where(a => a.IsRendered == YesNo.No && a.IsPaid == YesNo.Yes && a.ClientId == userId)
				.Include(a => a.Client).Include(a => a.Group)
					.Include(a => a.Service).Include(a => a.Worker).ThenInclude(w => w.User)
						.ToListAsync();
			return attendances;
		}

		//USER NOT RENDERED + NOT PAID

		public async Task<IEnumerable<Attendance>> MyNotRenderedNotPaidAttendances(string userId)
		{
			var attendances = await _context.Attendances.Where(a => a.IsRendered == YesNo.No && a.IsPaid == YesNo.No && a.ClientId == userId)
				.Include(a => a.Client).Include(a => a.Group)
					.Include(a => a.Service).Include(a => a.Worker).ThenInclude(w => w.User)
						.ToListAsync();
			return attendances;
		}


		//USER HISTORY (IS RENDERED)
		public async Task<IEnumerable<Attendance>> MyIsRenderedAttendances(string userId)
		{
			var attendances = await _context.Attendances.Where(a => a.IsRendered == YesNo.Yes && a.ClientId == userId)
				.Include(a => a.Client).Include(a => a.Group)
					.Include(a => a.Service).Include(a => a.Worker).ThenInclude(w => w.User)
						.ToListAsync();
			return attendances;
		}


		//WORKER'S DATA
		//NOT RENDERED IS PAID
		public async Task<IEnumerable<Attendance>> WorkerNotRenderedIsPaidAttendances(string workerId)
		{
			var attendances = await _context.Attendances.Where(a => a.WorkerId == workerId && a.IsRendered == YesNo.No && a.IsPaid == YesNo.Yes)
			.Include(a => a.Client).Include(a => a.Group)
			   .Include(a => a.Service).Include(a => a.Worker).ThenInclude(w => w.User).ToListAsync();
			return attendances;
		}

		//WORKER'S NOT RENDERED NOT PAID
		public async Task<IEnumerable<Attendance>> WorkerNotRenderedNotPaidAttendances(string workerId)
		{
			var attendances = await _context.Attendances.Where(a => a.WorkerId == workerId && a.IsRendered == YesNo.No && a.IsPaid == YesNo.No)
			.Include(a => a.Client).Include(a => a.Group)
			   .Include(a => a.Service).Include(a => a.Worker).ThenInclude(w => w.User).ToListAsync();
			return attendances;
		}

		//WORKER'S HISTORY CLIENTS
		public async Task<IEnumerable<Attendance>> WorkerIsRenderedAttendances(string workerId)
		{
			var attendances = await _context.Attendances.Where(a => a.WorkerId == workerId)
			   .Include(a => a.Client).Include(a => a.Group)
				   .Include(a => a.Service).Include(a => a.Worker).ThenInclude(w => w.User)
			.ToListAsync();

			return attendances;
		}


		public async Task<IEnumerable<TimeSpan>> GetFreeTimeSlots(string workerId, DateTime date)
		{
			List<TimeSpan> slots = new List<TimeSpan>();

			var day = date.DayOfWeek;

			var attendances = _context.Attendances.Where(a => a.WorkerId == workerId && a.Date == date).ToList();

			TimeSpan start = _context.Schedules.Where(s => s.WorkerId == workerId && s.Day == day.ToString()).Select(a => a.Start).FirstOrDefault();
			TimeSpan end = _context.Schedules.Where(s => s.WorkerId == workerId && s.Day == day.ToString()).Select(a => a.End).FirstOrDefault();

			var slotsTaken = attendances.Select(a => a.Time).ToList();

			for(TimeSpan i = start; i < end; i = new TimeSpan(i.Hours + 1, minutes: i.Minutes, seconds: i.Seconds))
			{
				if (slotsTaken.Where(s => s == i ).Count() > 0)
					continue;
				slots.Add(i);
			}

			return slots;
		}

		public async Task UpdateAttendances(List<Attendance> attendances)
		{
			foreach(var attendanceVM in attendances)
			{
				var attendance = _context.Attendances.Where(a => a.Id == attendanceVM.Id).FirstOrDefault();

				attendance.Date = attendanceVM.Date;
				attendance.Time = attendanceVM.Time;
				attendance.IsRendered = attendanceVM.IsRendered;
				attendance.IsPaid = attendanceVM.IsPaid;
				attendance.ServiceId = attendanceVM.ServiceId;
				attendance.GroupId = attendanceVM.GroupId;

				await _context.SaveChangesAsync();
			}
		}


		public async Task UpdateAttendance(Attendance attendanceVM)
		{
			var attendance = _context.Attendances.Where(a => a.Id == attendanceVM.Id).FirstOrDefault();

			attendance.Date = attendanceVM.Date;
			attendance.Time = attendanceVM.Time;
			attendance.IsRendered = attendanceVM.IsRendered;
			attendance.IsPaid = attendanceVM.IsPaid;
			attendance.ServiceId = attendanceVM.ServiceId;
			attendance.GroupId = attendanceVM.GroupId;

			await _context.SaveChangesAsync();
			
		}
	}
}
