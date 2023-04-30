﻿using hhSalon.Domain.Abstract;
using hhSalon.Domain.Entities;
using hhSalon.Services.ViewModels;

namespace hhSalon.Services.Services.Interfaces
{
    public interface IAttendancesService : IEntityBaseRepository<Attendance>
	{

		Task AddNewAttendanceAsync(NewAttendanceVM newAttendance);

		//Task<Attendance> GetAttendanceById(int id);
		//Task<IEnumerable<Attendance>> GetIsRenderedAttendances(); 
		//Task<IEnumerable<Attendance>> GetNotRenderedAttendances();

		// MY ATTENDANCES
		Task<IEnumerable<Attendance>> MyIsRenderedAttendances(string userId); //
		Task<IEnumerable<Attendance>> MyNotRenderedIsPaidAttendances(string userId); //
		Task<IEnumerable<Attendance>> MyNotRenderedNotPaidAttendances(string userId); //


		//WORKER'S DATA
		Task<IEnumerable<Attendance>> WorkerNotRenderedIsPaidAttendances(string workerId);
		Task<IEnumerable<Attendance>> WorkerNotRenderedNotPaidAttendances(string workerId);
		Task<IEnumerable<Attendance>> WorkerIsRenderedAttendances(string workerId);

		Task<IEnumerable<TimeSpan>> GetFreeTimeSlots(string workerId, DateTime date);

		//Task<IEnumerable<Attendance>> GeAllAttendances(string userId, string role);

		//Task CompletePaymentAll(string userId);

		//double GetTotal(string userId);
	}
}
