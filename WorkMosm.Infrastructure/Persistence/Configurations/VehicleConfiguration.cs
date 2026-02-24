using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkMosm.Domain.Entities;

namespace WorkMosm.Infrastructure.Persistence.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Brand).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Model).IsRequired().HasMaxLength(50);
        }
    }
}
