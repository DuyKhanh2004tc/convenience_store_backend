using ConvenienceStore.Application.DTOs.Supplier;

using ConvenienceStore.Application.DTOs.Common;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface ISupplierService
{
    Task<PagedResult<SupplierDto>> GetAllSuppliersAsync(int page = 1, int pageSize = 10);
    Task<SupplierDto?> GetSupplierByIdAsync(int id);
    Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
    Task<bool> UpdateSupplierAsync(int id, CreateSupplierDto dto);
    Task<bool> DeleteSupplierAsync(int id);
}
