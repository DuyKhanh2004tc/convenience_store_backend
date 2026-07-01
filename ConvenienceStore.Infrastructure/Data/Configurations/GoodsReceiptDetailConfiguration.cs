using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConvenienceStore.Infrastructure.Data.Configurations;

public class GoodsReceiptDetailConfiguration : IEntityTypeConfiguration<GoodsReceiptDetail>
{
    public void Configure(EntityTypeBuilder<GoodsReceiptDetail> builder)
    {
        builder.ToTable("GoodsReceiptDetails");
        
        // Composite Primary Key
        builder.HasKey(x => new { x.ReceiptId, x.ProductId });

        // Relationships
        builder.HasOne(x => x.GoodsReceipt)
            .WithMany(gr => gr.GoodsReceiptDetails)
            .HasForeignKey(x => x.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
