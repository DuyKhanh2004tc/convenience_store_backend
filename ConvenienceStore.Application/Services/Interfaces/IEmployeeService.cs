using ConvenienceStore.Application.DTOs.Employee;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<IEnumerable<EmployeeDto>> GetEmployeesByBranchAsync(int branchId);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(int userId);
    Task<bool> UpdateEmployeeAsync(int userId, UpdateEmployeeDto dto);
    Task<bool> ToggleEmployeeStatusAsync(int userId);
}
