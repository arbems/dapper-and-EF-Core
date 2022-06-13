using DapperAndEFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dapper.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();

        builder.Property(t => t.Username)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .OwnsOne(b => b.Address);
    }
}
