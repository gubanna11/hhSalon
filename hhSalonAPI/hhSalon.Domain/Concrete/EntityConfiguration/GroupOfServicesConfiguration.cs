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
	public class GroupOfServicesConfiguration : IEntityTypeConfiguration<GroupOfServices>
	{
		public void Configure(EntityTypeBuilder<GroupOfServices> builder)
		{
			builder.HasIndex(g => g.Name).IsUnique();
		}
	}
}
