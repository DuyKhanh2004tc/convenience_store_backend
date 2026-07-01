using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConvenienceStore.Infrastructure.Data.Configurations;

public class PurchaseOrderDetailConfiguration : IEntityTypeConfiguration<PurchaseOrderDetail>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderDetail> builder)
    {
        builder.ToTable("PurchaseOrderDetails");
        
        // Composite Primary Key
        builder.HasKey(x => new { x.PoId, x.ProductId });
        
        builder.Property(x => x.Quantity).HasDefaultValue(1);
        builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);

        // Relationships
        builder.HasOne(x => x.PurchaseOrder)
            .WithMany(po => po.PurchaseOrderDetails)
            .HasForeignKey(x => x.PoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
