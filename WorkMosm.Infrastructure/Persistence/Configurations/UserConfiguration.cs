using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkMosm.Domain.Entities;

namespace WorkMosm.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PasswordHash).IsRequired();

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);
        }
    }
}
