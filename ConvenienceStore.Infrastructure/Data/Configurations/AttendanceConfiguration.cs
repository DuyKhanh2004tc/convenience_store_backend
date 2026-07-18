using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConvenienceStore.Infrastructure.Data.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("Attendances");
        builder.HasKey(x => x.AttendanceId);

        // Enum to string conversions
        builder.Property(x => x.CheckInStatus).HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.CheckOutStatus).HasConversion<string>().HasMaxLength(50);

        // Check-in location data
        builder.Property(x => x.CheckInIp).HasMaxLength(50);
        builder.Property(x => x.CheckInWifiBssid).HasMaxLength(100);
        builder.Property(x => x.CheckInLatitude).HasColumnType("float");
        builder.Property(x => x.CheckInLongitude).HasColumnType("float");

        // Check-out location data
        builder.Property(x => x.CheckOutIp).HasMaxLength(50);
        builder.Property(x => x.CheckOutLatitude).HasColumnType("float");
        builder.Property(x => x.CheckOutLongitude).HasColumnType("float");

        // Timing
        builder.Property(x => x.LateMinutes).HasDefaultValue(0);
        builder.Property(x => x.EarlyMinutes).HasDefaultValue(0);

        // 1-to-1 relationship with ShiftAssignment
        builder.HasOne(x => x.ShiftAssignment)
            .WithOne(sa => sa.Attendance)
            .HasForeignKey<Attendance>(x => x.ShiftAssignmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ensure one attendance per shift assignment
        builder.HasIndex(x => x.ShiftAssignmentId).IsUnique();
    }
}
