using ConvenienceStore.Application.DTOs.Customer;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface ICustomerService
{
    /// <summary>
    /// Search customer by phone number (POS staff uses this when customer gives their phone).
    /// </summary>
    Task<CustomerDto?> SearchByPhoneAsync(string phone);

    /// <summary>
    /// Search customer by membership ID (scan membership card).
    /// </summary>
    Task<CustomerDto?> SearchByMembershipIdAsync(string membershipId);

    /// <summary>
    /// Register a new loyalty customer at POS.
    /// </summary>
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto request);

    /// <summary>
    /// Get customer by ID.
    /// </summary>
    Task<CustomerDto?> GetByIdAsync(int customerId);
}
