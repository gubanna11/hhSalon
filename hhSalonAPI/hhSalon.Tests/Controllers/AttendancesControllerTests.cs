using hhSalon.Domain.Abstract.Implementations;
using hhSalon.Domain.Abstract.Interfaces;
using hhSalon.Domain.Entities;
using hhSalon.Domain.Entities.Static;
using hhSalon.Services.Services.Implementations;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Controllers;
using hhSalonAPI.Domain.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhSalon.Tests.Controllers
{
    public class AttendancesControllerTests
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "AttendancesControllerTest")
            .Options;

        AppDbContext context;
        AttendancesController attendancesController;
        IUnitOfWork unitOfWork;

        AttendancesService attendancesService;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            unitOfWork = new UnitOfWork(context);
            context.Database.EnsureCreated();

            SeedDatabase();

            attendancesService = new AttendancesService(unitOfWork);
            attendancesController = new AttendancesController(attendancesService);
        }


        [Test, Order(1)]
        public async Task AttendancesController_NewAttendance_ReturnOk()
        {
            NewAttendanceVM attendanceVM = new NewAttendanceVM()
            {
                GroupId = 1,
                ServiceId = 1,
                ClientId = "2",
                WorkerId = "1",
                IsPaid = YesNo.No,
                Date = new DateTime(2023, 1, 2)
            };

            IActionResult actionResult = await attendancesController.NewAttendance(attendanceVM);

            Assert.That(actionResult, Is.TypeOf<OkResult>());

            Assert.That(context.Attendances.Count, Is.EqualTo(2));
            Assert.That(context.Attendances.Last().ClientId, Is.EqualTo("2"));
        }


        [Test, Order(2)]
        public async Task AttendancesController_NewAttendance_ReturnBadRequest()
        {
            NewAttendanceVM attendanceVM = new NewAttendanceVM()
            {
                GroupId = 1,
                ServiceId = 1,
                ClientId = "2",
                WorkerId = "3",
                Date = new DateTime(2023, 1, 2)
            };

            IActionResult actionResult = await attendancesController.NewAttendance(attendanceVM);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(3)]
        public async Task AttendancesController_DeleteAttendance_ReturnOk()
        {
            int id = 2;
            IActionResult actionResult = await attendancesController.DeleteAttendance(id);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            Assert.That(context.Attendances.Count, Is.EqualTo(1));
            Assert.That(context.Attendances.Last().ClientId, Is.EqualTo("1"));
        }



        [Test, Order(4)]
        public async Task AttendancesController_DeleteAttendance_ReturnNotFound()
        {
            int id = 222;
            IActionResult actionResult = await attendancesController.DeleteAttendance(id);

            Assert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());
        }



        public void SeedDatabase()
        {
            var attendances = new List<Attendance>
            {
                new Attendance()
                {
                    Id = 1,
                    GroupId = 1,
                    ServiceId = 1,
                    ClientId = "1",
                    WorkerId = "1",
                    IsPaid = YesNo.No,
                    Date = new DateTime(2023, 1, 1)
                }
            };

            context.Attendances.AddRange(attendances);
            context.SaveChanges();
        }
    }
}
