using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConvenienceStore.Infrastructure.Data.Configurations;

public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
{
    public void Configure(EntityTypeBuilder<GoodsReceipt> builder)
    {
        builder.ToTable("GoodsReceipts");
        builder.HasKey(x => x.ReceiptId);

        builder.Property(x => x.Notes).HasMaxLength(500);

        // Relationships
        builder.HasOne(x => x.PurchaseOrder)
            .WithMany(po => po.GoodsReceipts)
            .HasForeignKey(x => x.PoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Receiver)
            .WithMany()
            .HasForeignKey(x => x.ReceivedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
