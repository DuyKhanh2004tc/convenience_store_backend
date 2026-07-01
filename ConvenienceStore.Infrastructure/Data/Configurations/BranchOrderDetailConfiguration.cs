using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConvenienceStore.Infrastructure.Data.Configurations;

public class BranchOrderDetailConfiguration : IEntityTypeConfiguration<BranchOrderDetail>
{
    public void Configure(EntityTypeBuilder<BranchOrderDetail> builder)
    {
        builder.ToTable("BranchOrderDetails");
        
        // Composite Primary Key
        builder.HasKey(x => new { x.OrderId, x.ProductId });
        
        builder.Property(x => x.Quantity).HasDefaultValue(1);

        // Relationships
        builder.HasOne(x => x.BranchOrder)
            .WithMany(bo => bo.BranchOrderDetails)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
