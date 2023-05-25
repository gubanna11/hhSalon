

using hhSalon.Domain.Entities;
using hhSalon.Services.Services.Implementations;
using hhSalon.Services.ViewModels;
using hhSalonAPI.Controllers;
using hhSalonAPI.Domain.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hhSalon.Tests.Services
{
	public class Tests
	{
		private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
					.UseInMemoryDatabase(databaseName: "ServicesControllerTest")
					.Options;

		AppDbContext context;
		ServicesController servicesController;

		ServicesService servicesService;

		[OneTimeSetUp]
		public void Setup()
		{
			context = new AppDbContext(dbContextOptions);
			context.Database.EnsureCreated();

			SeedDatabase();

			servicesService = new ServicesService(context);
			servicesController = new ServicesController(servicesService);
		}

		[Test, Order(1)]
		public async Task HTTPGET_GetServicesByGroupIdAsync_ReturnOk()
		{
			int id = 1;
			IActionResult actionResult = await servicesController.GetServiceVMsByGroupId(id);

			Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

			var actionResultData = (actionResult as OkObjectResult).Value as List<ServiceVM>;

			Assert.That(actionResultData.First().Name, Is.EqualTo("Service 1"));
			Assert.That(actionResultData.First().Id, Is.EqualTo(1));
			Assert.That(actionResultData.Count, Is.EqualTo(3));
		}


		[Test, Order(2)]
		public async Task HTTPGET_GetServicesByGroupIdAsync_ReturnNotFound()
		{
			int id = 111;
			IActionResult actionResult = await servicesController.GetServiceVMsByGroupId(id);

			Assert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());
		}


		[Test, Order(3)]
		public async Task HTTPPUT_CreateService_ReturnOk()
		{
			ServiceVM serviceVM = new ServiceVM()
			{
				GroupId = 1,
				Name = "Service 5",
				Price = 5,
			};
			IActionResult actionResult = await servicesController.CreateService(serviceVM);

			Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

			Assert.That(context.Services.Count, Is.EqualTo(5));
			Assert.That(context.Services.Last().Name, Is.EqualTo("Service 5"));
			Assert.That(context.Services.Include(s => s.ServiceGroup).Where(s => s.ServiceGroup.GroupId == serviceVM.GroupId
				&& s == context.Services.Last()).ToList().Count, Is.EqualTo(1));
		}


		[Test, Order(4)]
		public async Task HTTPPUT_UpdateService_ReturnOk()
		{
			ServiceVM serviceVM = new ServiceVM()
			{
				Id = 5,
				Name = "Service 5 updated",
				Price = 5,
				GroupId = 2,
			};
			IActionResult actionResult = await servicesController.UpdateService(serviceVM);

			Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

			Assert.That(context.Services.Last().Name, Is.EqualTo("Service 5 updated"));
			Assert.That(context.Services_Groups.Where(sg => sg.ServiceId == serviceVM.Id && sg.GroupId == serviceVM.GroupId).Count,
				Is.EqualTo(1));
		}

		[Test, Order(5)]
		public async Task HTTPPUT_UpdateService_ReturnBadRequest()
		{
			ServiceVM serviceVM = new ServiceVM()
			{
				Id = 999,
				Name = "Service 5 updated",
				Price = 5,
				GroupId = 2,
			};
			IActionResult actionResult = await servicesController.UpdateService(serviceVM);

			Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
		}



		[Test, Order(6)]
		public async Task HTTPDELETE_DeleteService_ReturnOk()
		{
			IActionResult actionResult = await servicesController.DeleteService(5);

			Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

			Assert.That(context.Services.Count, Is.EqualTo(4));
			Assert.That(context.Services.Last().Name, Is.EqualTo("Service 4"));
		}


		[Test, Order(7)]
		public async Task HTTPDELETE_DeleteService_ReturnBadRequest()
		{
			IActionResult actionResult = await servicesController.DeleteService(999);

			Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
		}




		[OneTimeTearDown]
		public void ClearUp()
		{
			context.Database.EnsureDeleted();
		}

		private void SeedDatabase()
		{
			var services = new List<Service>
			{
				new Service()
				{
					Id = 1,
					Name = "Service 1",
					Price = 1
				},
				new Service()
				{
					Id = 2,
					Name = "Service 2",
					Price = 2
				},
				new Service()
				{
					 Id = 3,
					Name = "Service 3",
					Price = 3
				},
				new Service()
				{
					 Id = 4,
					Name = "Service 4",
					Price = 4
				},
			};


			var services_groups = new List<ServiceGroup>
			{
				new ServiceGroup()
				{
					ServiceId = 1,
					GroupId = 1
				},
				new ServiceGroup()
				{
					ServiceId = 2,
					GroupId = 3
				},
				new ServiceGroup()
				{
					ServiceId = 3,
					GroupId = 1
				},
				new ServiceGroup()
				{
					ServiceId = 4,
					GroupId = 1
				}
			};

			context.Services.AddRange(services);
			context.SaveChanges();

			services.ForEach(s => context.Entry(s).State = EntityState.Detached);

			context.Services_Groups.AddRange(services_groups);
			context.SaveChanges();
			//services.ForEach(s => context.Entry(s).State = EntityState.Detached);
		}
	}
}