using hhSalon.Domain.Entities;
using hhSalonAPI.Domain.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace hhSalon.Domain.Concrete
{
	public static class AppDbInitializer
	{
		public static void Seed(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var _context = serviceScope.ServiceProvider.GetService<AppDbContext>();

				if (!_context.Groups.Any())
				{
					_context.Groups.AddRange(new List<GroupOfServices>()
					{
						new GroupOfServices()
						{
							Name = "Cosmetology"
						},
						new GroupOfServices()
						{
							Name = "Massages"
						},
						new GroupOfServices()
						{
							Name = "Nail service"
						},
						new GroupOfServices()
						{
							Name = "Hairdresser's"
						},
						new GroupOfServices()
						{
							Name = "Makeup"
						},
						new GroupOfServices()
						{
							Name = "Depilation"
						}
					});

					_context.SaveChanges();
				}

				if (!_context.Services.Any())
				{
					_context.Services.AddRange(new List<Service>()
					{
						new Service()
						{
							Name = "Eyebrow correction",
							Price = 80
						},
						new Service()
						{
							Name = "Ultrasonic cleaning",
							Price = 600
						},
						new Service()
						{
							Name = "Mechanical cleaning",
							Price = 500
						},
						new Service()
						{
							Name = "Combined facial cleansing",
							Price = 550
						},
						new Service()
						{
							Name = "Facial massage",
							Price = 200
						},
						new Service()
						{
							Name = "Mechanical peeling",
							Price = 600
						},
						new Service()
						{
							Name = "Chemical peeling",
							Price = 550
						},
						new Service()
						{
							Name = "Therapeutic massage",
							Price = 400
						},
						new Service()
						{
							Name = "Classic massage",
							Price = 300
						},
						new Service()
						{
							Name = "Lymphatic drainage massage",
							Price = 500
						},
						new Service()
						{
							Name = "Manicure",
							Price = 200
						},
						new Service()
						{
							Name = "Covering with gel varnish",
							Price = 180
						},
						new Service()
						{
							Name = "Shellac",
							Price = 200
						},
						new Service()
						{
							Name = "Pedicure",
							Price = 300
						},
						new Service()
						{
							Name = "Children's haircut",
							Price = 80
						},
						new Service()
						{
							Name = "Women's haircut",
							Price = 120
						},
						new Service()
						{
							Name = "Men's haircut",
							Price = 100
						},
						new Service()
						{
							Name = "Dyeing",
							Price = 350
						},
						new Service()
						{
							Name = "Drawing",
							Price = 600
						},
						new Service()
						{
							Name = "Day makeup",
							Price = 450
						},
						new Service()
						{
							Name = "Evening makeup",
							Price = 500
						},
						new Service()
						{
							Name = "Makeup express",
							Price = 300
						},
						new Service()
						{
							Name = "Waxing - legs",
							Price = 200
						},
						new Service()
						{
							Name = "Waxing - hands",
							Price = 170
						},
						new Service()
						{
							Name = "Waxing - bikini",
							Price = 250
						},
					});
					_context.SaveChanges();
				}

				if (!_context.Services_Groups.Any())
				{
					_context.Services_Groups.AddRange(new List<ServiceGroup>()
					{
						new ServiceGroup()
						{
							ServiceId = 1,
							GroupId = 1
						},
						new ServiceGroup()
						{
							ServiceId = 2,
							GroupId = 1
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
						},
						new ServiceGroup()
						{
							ServiceId = 5,
							GroupId = 1
						},
						new ServiceGroup()
						{
							ServiceId = 6,
							GroupId = 1
						},
						new ServiceGroup()
						{
							ServiceId = 7,
							GroupId = 1
						},
						new ServiceGroup()
						{
							ServiceId = 8,
							GroupId = 2
						},
						new ServiceGroup()
						{
							ServiceId = 9,
							GroupId = 2
						},
						new ServiceGroup()
						{
							ServiceId = 10,
							GroupId = 2
						},
						new ServiceGroup()
						{
							ServiceId = 11,
							GroupId = 3
						},
						new ServiceGroup()
						{
							ServiceId = 12,
							GroupId = 3
						},
						new ServiceGroup()
						{
							ServiceId = 13,
							GroupId = 3
						},
						new ServiceGroup()
						{
							ServiceId = 14,
							GroupId = 3
						},
						new ServiceGroup()
						{
							ServiceId = 15,
							GroupId = 4
						},
						new ServiceGroup()
						{
							ServiceId = 16,
							GroupId = 4
						},
						new ServiceGroup()
						{
							ServiceId = 17,
							GroupId = 4
						},
						new ServiceGroup()
						{
							ServiceId = 18,
							GroupId = 4
						},
						new ServiceGroup()
						{
							ServiceId = 19,
							GroupId = 4
						},
						new ServiceGroup()
						{
							ServiceId = 20,
							GroupId = 5
						},
						new ServiceGroup()
						{
							ServiceId = 21,
							GroupId = 5
						},
						new ServiceGroup()
						{
							ServiceId = 22,
							GroupId = 5
						},
						new ServiceGroup()
						{
							ServiceId = 23,
							GroupId = 6
						},
						new ServiceGroup()
						{
							ServiceId = 24,
							GroupId = 6
						},
						new ServiceGroup()
						{
							ServiceId = 25,
							GroupId = 6
						},
					});

					_context.SaveChanges();
				}

				//if (!context.Attendances.Any())
				//{
				//	context.Attendances.AddRange(new List<Attendance>()
				//	{
				//		new Attendance()
				//		{
				//			ClientId = context.Users.FirstOrDefault().Id,
				//			GroupId = 1,
				//			ServiceId = 1,
				//			WorkerId = context.Workers_Groups.Where(g => g.GroupId == 1).FirstOrDefault().WorkerId,
				//			Date = new DateTime(2023, 2, 20),
				//			Time = new TimeSpan(12, 0, 0),
				//			Price = context.Services.Where(s => s.Id == 1).FirstOrDefault().Price,
				//			IsRendered = YesNoEnum.No,
				//			IsPaid = YesNoEnum.No
				//		},
				//		new Attendance()
				//		{
				//			ClientId = context.Users.FirstOrDefault().Id,
				//			GroupId = 2,
				//			ServiceId = 8,
				//			WorkerId = context.Workers_Groups.Where(g => g.GroupId == 2).FirstOrDefault().WorkerId,
				//			Date = new DateTime(2023, 3, 20),
				//			Time = new TimeSpan(14, 0, 0),
				//			Price = context.Services.Where(s => s.Id == 8).FirstOrDefault().Price,
				//			IsRendered = YesNoEnum.No,
				//			IsPaid = YesNoEnum.Yes
				//		}
				//	});
				//}
			}
		}

	}
}
