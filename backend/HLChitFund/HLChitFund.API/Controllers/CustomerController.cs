using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HLChitFund.API.Controllers;

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

    [HttpGet]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService
            .GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerService
            .GetCustomerByIdAsync(id);

        if (customer == null)
            return NotFound(new { message = "Customer not found!" });

        return Ok(customer);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CustomerRequestDto request)
    {
        try
        {
            var customer = await _customerService
                .CreateCustomerAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = customer.Id }, customer);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id, [FromBody] CustomerRequestDto request)
    {
        try
        {
            var customer = await _customerService
                .UpdateCustomerAsync(id, request);
            return Ok(customer);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _customerService.DeleteCustomerAsync(id);
            return Ok(new { message = "Customer deleted successfully!" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/toggle-status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            await _customerService.ToggleCustomerStatusAsync(id);
            return Ok(new { message = "Customer status updated!" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}