using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Common;
using ConvenienceStore.Application.DTOs.Supplier;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly IGenericRepository<Supplier> _supplierRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SupplierService(
        IGenericRepository<Supplier> supplierRepository,
        IUnitOfWork unitOfWork)
    {
        _supplierRepository = supplierRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<SupplierDto>> GetAllSuppliersAsync(int page = 1, int pageSize = 10)
    {
        var allSuppliers = await _supplierRepository.GetAllAsync();
        
        int totalCount = allSuppliers.Count();
        var pagedSuppliers = allSuppliers.Skip((page - 1) * pageSize).Take(pageSize);

        var items = pagedSuppliers.Select(s => new SupplierDto
        {
            SupplierId = s.SupplierId,
            SupplierName = s.SupplierName,
            ContactName = s.ContactName,
            Phone = s.Phone,
            Email = s.Email,
            Address = s.Address,
            IsActive = s.IsActive
        }).ToList();

        return new PagedResult<SupplierDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public async Task<SupplierDto?> GetSupplierByIdAsync(int id)
    {
        var s = await _supplierRepository.GetByIdAsync(id);
        if (s == null) return null;

        return new SupplierDto
        {
            SupplierId = s.SupplierId,
            SupplierName = s.SupplierName,
            ContactName = s.ContactName,
            Phone = s.Phone,
            Email = s.Email,
            Address = s.Address,
            IsActive = s.IsActive
        };
    }

    public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto)
    {
        var supplier = new Supplier
        {
            SupplierName = dto.SupplierName,
            ContactName = dto.ContactName,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            IsActive = dto.IsActive
        };

        var created = await _supplierRepository.AddAsync(supplier);
        await _unitOfWork.SaveChangesAsync();

        return new SupplierDto
        {
            SupplierId = created.SupplierId,
            SupplierName = created.SupplierName,
            ContactName = created.ContactName,
            Phone = created.Phone,
            Email = created.Email,
            Address = created.Address,
            IsActive = created.IsActive
        };
    }

    public async Task<bool> UpdateSupplierAsync(int id, CreateSupplierDto dto)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null) return false;

        supplier.SupplierName = dto.SupplierName;
        supplier.ContactName = dto.ContactName;
        supplier.Phone = dto.Phone;
        supplier.Email = dto.Email;
        supplier.Address = dto.Address;
        supplier.IsActive = dto.IsActive;

        _supplierRepository.Update(supplier);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null) return false;

        _supplierRepository.Delete(supplier);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
