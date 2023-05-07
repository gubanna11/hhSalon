using hhSalon.Domain.Concrete.EntityConfiguration;
using hhSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace hhSalonAPI.Domain.Concrete
{
	public class AppDbContext: DbContext
    {
		public AppDbContext(DbContextOptions options) : base(options) { }

		public DbSet<Service> Services { get; set; }
        public DbSet<GroupOfServices> Groups { get; set; }
        public DbSet<ServiceGroup> Services_Groups { get; set; }

        public DbSet<Worker> Workers { get; set; }

        public DbSet<WorkerGroup> Workers_Groups { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Chat> Chats { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.ApplyConfiguration(new AttendanceConfiguration());
			modelBuilder.ApplyConfiguration(new GroupOfServicesConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());

			/**/
			modelBuilder.Entity<ServiceGroup>().HasKey(sg => new { sg.ServiceId, sg.GroupId });

            modelBuilder.Entity<ServiceGroup>().HasOne(g => g.Group).WithMany(sg => sg.Services_Groups);

            modelBuilder.Entity<ServiceGroup>().HasOne(s => s.Service).WithOne(sg => sg.ServiceGroup);

            /**/
            modelBuilder.Entity<WorkerGroup>().HasKey(wg => new { wg.WorkerId, wg.GroupId });

			modelBuilder.Entity<WorkerGroup>().HasOne(w => w.Group).WithMany(wg => wg.Workers_Groups);

            
			
            modelBuilder.Entity<Schedule>().HasOne(s => s.Worker).WithMany(s => s.Schedules).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().ToTable("users");
            


			base.OnModelCreating(modelBuilder);
        }

    }
}
