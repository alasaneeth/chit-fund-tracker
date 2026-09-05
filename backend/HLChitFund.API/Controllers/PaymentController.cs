using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HLChitFund.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetAll()
    {
        var payments = await _paymentService.GetAllPaymentsAsync();
        return Ok(payments);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetById(int id)
    {
        var payment = await _paymentService.GetPaymentByIdAsync(id);

        if (payment == null)
            return NotFound(new { message = "Payment not found!" });

        return Ok(payment);
    }

    [HttpGet("enrollment/{enrollmentId}")]
    [Authorize(Roles = "Admin,Cashier,Customer")]
    public async Task<IActionResult> GetByEnrollment(
        int enrollmentId)
    {
        var payments = await _paymentService
            .GetPaymentsByEnrollmentAsync(enrollmentId);
        return Ok(payments);
    }

    [HttpGet("chit-group/{chitGroupId}")]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetByChitGroup(int chitGroupId)
    {
        var payments = await _paymentService
            .GetPaymentsByChitGroupAsync(chitGroupId);
        return Ok(payments);
    }

    [HttpPost("collect")]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> CollectPayment(
        [FromBody] PaymentRequestDto request)
    {
        try
        {
            var payment = await _paymentService
                .CollectPaymentAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = payment.Id }, payment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _paymentService.DeletePaymentAsync(id);
            return Ok(new
            {
                message = "Payment deleted successfully!"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}