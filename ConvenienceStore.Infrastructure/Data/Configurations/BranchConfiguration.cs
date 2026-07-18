using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConvenienceStore.Infrastructure.Data.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");
        builder.HasKey(x => x.BranchId);

        builder.Property(x => x.BranchName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        // GPS coordinates for attendance geo-fencing
        builder.Property(x => x.Latitude).HasColumnType("float");
        builder.Property(x => x.Longitude).HasColumnType("float");

        // Allowed WiFi MAC addresses (comma-separated)
        builder.Property(x => x.AllowedWifiBssid).HasMaxLength(500);
    }
}
