using hhSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhSalon.Domain.Concrete.EntityConfiguration
{
	public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
	{
		public void Configure(EntityTypeBuilder<Attendance> builder)
		{
			builder.HasIndex(att => new { att.ClientId, att.Date, att.ServiceId }).IsUnique();
			builder.HasIndex(att => new { att.WorkerId, att.Date, att.ServiceId }).IsUnique();
			builder.Property(a => a.Time).HasColumnType("time");
			builder.Property(a => a.Date).HasColumnType("date");
		}
	}
}
