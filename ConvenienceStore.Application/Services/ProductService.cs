using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Product;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IGenericRepository<Supplier> _supplierRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(
        IGenericRepository<Product> productRepository,
        IGenericRepository<Category> categoryRepository,
        IGenericRepository<Supplier> supplierRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _supplierRepository = supplierRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(string? search = null, int? categoryId = null)
    {
        // Fetch products matching filter criteria
        var products = await _productRepository.GetAsync(p =>
            (string.IsNullOrEmpty(search) || p.ProductName.Contains(search)) &&
            (!categoryId.HasValue || p.CategoryId == categoryId.Value)
        );

        // Fetch categories and suppliers to populate names in memory
        var categories = (await _categoryRepository.GetAllAsync()).ToDictionary(c => c.CategoryId, c => c.CategoryName);
        var suppliers = (await _supplierRepository.GetAllAsync()).ToDictionary(s => s.SupplierId, s => s.SupplierName);

        return products.Select(p => new ProductDto
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            CategoryId = p.CategoryId,
            CategoryName = categories.TryGetValue(p.CategoryId, out var catName) ? catName : string.Empty,
            SupplierId = p.SupplierId,
            SupplierName = p.SupplierId.HasValue && suppliers.TryGetValue(p.SupplierId.Value, out var supName) ? supName : null,
            Barcode = p.Barcode,
            CostPrice = p.CostPrice,
            SellingPrice = p.SellingPrice,
            IsActive = p.IsActive,
            ImageUrl = p.ImageUrl,
            Unit = p.Unit,
            ImportUnit = p.ImportUnit,
            ConversionFactor = p.ConversionFactor
        }).ToList();
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var p = await _productRepository.GetByIdAsync(id);
        if (p == null) return null;

        var category = await _categoryRepository.GetByIdAsync(p.CategoryId);
        var supplier = p.SupplierId.HasValue ? await _supplierRepository.GetByIdAsync(p.SupplierId.Value) : null;

        return new ProductDto
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            CategoryId = p.CategoryId,
            CategoryName = category?.CategoryName ?? string.Empty,
            SupplierId = p.SupplierId,
            SupplierName = supplier?.SupplierName,
            Barcode = p.Barcode,
            CostPrice = p.CostPrice,
            SellingPrice = p.SellingPrice,
            IsActive = p.IsActive,
            ImageUrl = p.ImageUrl,
            Unit = p.Unit,
            ImportUnit = p.ImportUnit,
            ConversionFactor = p.ConversionFactor
        };
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            ProductName = dto.ProductName,
            CategoryId = dto.CategoryId,
            SupplierId = dto.SupplierId,
            Barcode = dto.Barcode,
            CostPrice = dto.CostPrice,
            SellingPrice = dto.SellingPrice,
            IsActive = dto.IsActive,
            ImageUrl = dto.ImageUrl,
            Unit = dto.Unit,
            ImportUnit = dto.ImportUnit,
            ConversionFactor = dto.ConversionFactor
        };

        var created = await _productRepository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        var category = await _categoryRepository.GetByIdAsync(created.CategoryId);
        var supplier = created.SupplierId.HasValue ? await _supplierRepository.GetByIdAsync(created.SupplierId.Value) : null;

        return new ProductDto
        {
            ProductId = created.ProductId,
            ProductName = created.ProductName,
            CategoryId = created.CategoryId,
            CategoryName = category?.CategoryName ?? string.Empty,
            SupplierId = created.SupplierId,
            SupplierName = supplier?.SupplierName,
            Barcode = created.Barcode,
            CostPrice = created.CostPrice,
            SellingPrice = created.SellingPrice,
            IsActive = created.IsActive,
            ImageUrl = created.ImageUrl,
            Unit = created.Unit,
            ImportUnit = created.ImportUnit,
            ConversionFactor = created.ConversionFactor
        };
    }

    public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return false;

        product.ProductName = dto.ProductName;
        product.CategoryId = dto.CategoryId;
        product.SupplierId = dto.SupplierId;
        product.Barcode = dto.Barcode;
        product.CostPrice = dto.CostPrice;
        product.SellingPrice = dto.SellingPrice;
        product.IsActive = dto.IsActive;
        product.ImageUrl = dto.ImageUrl;
        product.Unit = dto.Unit;
        product.ImportUnit = dto.ImportUnit;
        product.ConversionFactor = dto.ConversionFactor;

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return false;

        _productRepository.Delete(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
