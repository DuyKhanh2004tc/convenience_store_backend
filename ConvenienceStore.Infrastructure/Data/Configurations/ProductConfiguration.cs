using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConvenienceStore.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.ProductId);

        builder.Property(x => x.ProductName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Barcode).HasMaxLength(100);
        builder.HasIndex(x => x.Barcode).IsUnique();

        builder.Property(x => x.CostPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(x => x.SellingPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        // Relationships
        builder.HasOne(x => x.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
