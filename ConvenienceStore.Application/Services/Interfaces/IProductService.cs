using ConvenienceStore.Application.DTOs.Product;

using ConvenienceStore.Application.DTOs.Common;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetAllProductsAsync(string? search = null, int? categoryId = null, int page = 1, int pageSize = 10);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto?> GetProductByBarcodeAsync(string barcode);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task<bool> UpdateProductAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteProductAsync(int id);
}
