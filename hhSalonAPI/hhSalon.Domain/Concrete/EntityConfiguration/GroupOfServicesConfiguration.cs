using hhSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
