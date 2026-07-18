using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConvenienceStore.Infrastructure.Data.Configurations;

public class ShiftAssignmentConfiguration : IEntityTypeConfiguration<ShiftAssignment>
{
    public void Configure(EntityTypeBuilder<ShiftAssignment> builder)
    {
        builder.ToTable("ShiftAssignments");
        builder.HasKey(x => x.AssignmentId);

        builder.Property(x => x.WorkDate).HasColumnType("date").IsRequired();

        // Enum to string conversion
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);

        // Prevent double-booking at the database level
        builder.HasIndex(x => new { x.EmployeeId, x.ShiftId, x.WorkDate }).IsUnique();

        // Relationships
        builder.HasOne(x => x.Shift)
            .WithMany(s => s.ShiftAssignments)
            .HasForeignKey(x => x.ShiftId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Employee)
            .WithMany(u => u.ShiftAssignments)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // 1-to-1 with Attendance (configured from Attendance side)
    }
}
