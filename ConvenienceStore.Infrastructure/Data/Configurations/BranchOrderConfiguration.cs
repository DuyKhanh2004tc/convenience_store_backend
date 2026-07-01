using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConvenienceStore.Infrastructure.Data.Configurations;

public class BranchOrderConfiguration : IEntityTypeConfiguration<BranchOrder>
{
    public void Configure(EntityTypeBuilder<BranchOrder> builder)
    {
        builder.ToTable("BranchOrders");
        builder.HasKey(x => x.OrderId);

        // Enum to string conversion
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);

        // Relationships
        builder.HasOne(x => x.Branch)
            .WithMany(b => b.BranchOrders)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
