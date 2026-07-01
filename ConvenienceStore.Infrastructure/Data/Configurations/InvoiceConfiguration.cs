using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConvenienceStore.Infrastructure.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");
        builder.HasKey(x => x.InvoiceId);

        // Enum to string conversion
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);
        
        builder.Property(x => x.PaymentMethod).IsRequired().HasMaxLength(50);
        builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(x => x.FinalAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);

        // Relationships
        builder.HasOne(x => x.Branch)
            .WithMany(b => b.Invoices)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Staff)
            .WithMany()
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Customer)
            .WithMany(c => c.Invoices)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
