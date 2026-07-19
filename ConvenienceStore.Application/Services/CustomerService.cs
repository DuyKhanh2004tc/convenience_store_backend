using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Customer;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IGenericRepository<Customer> _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(
        IGenericRepository<Customer> customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDto?> SearchByPhoneAsync(string phone)
    {
        var customers = await _customerRepository.GetAsync(c => c.Phone == phone);
        var customer = customers.FirstOrDefault();
        return customer == null ? null : MapToDto(customer);
    }

    public async Task<CustomerDto?> SearchByMembershipIdAsync(string membershipId)
    {
        var customers = await _customerRepository.GetAsync(c => c.MembershipId == membershipId);
        var customer = customers.FirstOrDefault();
        return customer == null ? null : MapToDto(customer);
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto request)
    {
        // Validate phone uniqueness
        var existingPhone = await _customerRepository.GetAsync(c => c.Phone == request.Phone);
        if (existingPhone.Any())
            throw new Exception("A customer with this phone number already exists.");

        // Generate membership ID (e.g., "MEM-" + timestamp)
        var membershipId = $"MEM-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";

        var customer = new Customer
        {
            FullName = request.FullName,
            Phone = request.Phone,
            Email = request.Email,
            MembershipId = membershipId,
            LoyaltyPoints = 0
        };

        await _customerRepository.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(customer);
    }

    public async Task<CustomerDto?> GetByIdAsync(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        return customer == null ? null : MapToDto(customer);
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            FullName = customer.FullName,
            Phone = customer.Phone,
            Email = customer.Email,
            MembershipId = customer.MembershipId,
            LoyaltyPoints = customer.LoyaltyPoints,
            CreatedAt = customer.CreatedAt
        };
    }
}
