using ConvenienceStore.Application.DTOs.Product;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync(string? search = null, int? categoryId = null);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task<bool> UpdateProductAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteProductAsync(int id);
}
