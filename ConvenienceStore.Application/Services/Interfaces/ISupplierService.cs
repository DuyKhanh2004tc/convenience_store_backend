using ConvenienceStore.Application.DTOs.Supplier;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
    Task<SupplierDto?> GetSupplierByIdAsync(int id);
    Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
    Task<bool> UpdateSupplierAsync(int id, CreateSupplierDto dto);
    Task<bool> DeleteSupplierAsync(int id);
}
