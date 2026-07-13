using ConvenienceStore.Application.DTOs.Customer;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Search customer by phone number.
    /// POS staff calls this when customer reads out their phone number.
    /// </summary>
    [HttpGet("search/phone/{phone}")]
    [Authorize(Roles = "Store Staff, Store Manager")]
    public async Task<IActionResult> SearchByPhone(string phone)
    {
        try
        {
            var customer = await _customerService.SearchByPhoneAsync(phone);
            if (customer == null)
                return NotFound(new { StatusCode = 404, Message = "No customer found with this phone number." });

            return Ok(new { StatusCode = 200, Message = "Success", Data = customer });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Search customer by membership ID (scan membership card).
    /// </summary>
    [HttpGet("search/membership/{membershipId}")]
    [Authorize(Roles = "Store Staff, Store Manager")]
    public async Task<IActionResult> SearchByMembershipId(string membershipId)
    {
        try
        {
            var customer = await _customerService.SearchByMembershipIdAsync(membershipId);
            if (customer == null)
                return NotFound(new { StatusCode = 404, Message = "No customer found with this membership ID." });

            return Ok(new { StatusCode = 200, Message = "Success", Data = customer });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Register a new loyalty customer at POS.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Store Staff, Store Manager")]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto request)
    {
        try
        {
            var customer = await _customerService.CreateCustomerAsync(request);
            return StatusCode(201, new { StatusCode = 201, Message = "MSG-03: Customer registered successfully.", Data = customer });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Get customer by ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Store Staff, Store Manager, System Admin, Chain Director")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
                return NotFound(new { StatusCode = 404, Message = "Customer not found." });

            return Ok(new { StatusCode = 200, Message = "Success", Data = customer });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }
}
