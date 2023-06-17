using hhSalon.Domain.Abstract;
using hhSalon.Domain.Entities;
using hhSalon.Services.ViewModels;

namespace hhSalon.Services.Services.Interfaces
{
    public interface IAttendancesService : IEntityBaseRepository<Attendance>
	{
		Task<IEnumerable<Attendance>> GeAllAttendances();
		Task<IEnumerable<Attendance>> GeAttendancesBySearch(string content);

		Task AddNewAttendanceAsync(NewAttendanceVM newAttendance);


		// MY ATTENDANCES
		Task<IEnumerable<Attendance>> MyIsRenderedAttendances(string userId); //
		Task<IEnumerable<Attendance>> MyNotRenderedIsPaidAttendances(string userId); //
		Task<IEnumerable<Attendance>> MyNotRenderedNotPaidAttendances(string userId); //


		//WORKER'S DATA
		Task<IEnumerable<Attendance>> WorkerNotRenderedIsPaidAttendances(string workerId);
		Task<IEnumerable<Attendance>> WorkerNotRenderedNotPaidAttendances(string workerId);
        Task<IEnumerable<Attendance>> WorkerNotRenderedAttendances(string workerId);
        Task<IEnumerable<Attendance>> WorkerIsRenderedAttendances(string workerId);

		Task<IEnumerable<TimeSpan>> GetFreeTimeSlots(string workerId, DateTime date);


		Task UpdateAttendances(List<Attendance> attendanceVM);
		Task UpdateAttendance(Attendance attendanceVM);
	}
}
