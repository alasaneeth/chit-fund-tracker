using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Enrollment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HLChitFund.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentController(
        IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetAll()
    {
        var enrollments = await _enrollmentService
            .GetAllEnrollmentsAsync();
        return Ok(enrollments);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetById(int id)
    {
        var enrollment = await _enrollmentService
            .GetEnrollmentByIdAsync(id);

        if (enrollment == null)
            return NotFound(new { message = "Enrollment not found!" });

        return Ok(enrollment);
    }

    [HttpGet("chit-group/{chitGroupId}")]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetByChitGroup(int chitGroupId)
    {
        var enrollments = await _enrollmentService
            .GetEnrollmentsByChitGroupAsync(chitGroupId);
        return Ok(enrollments);
    }

    [HttpGet("customer/{customerId}")]
    [Authorize(Roles = "Admin,Cashier,Customer")]
    public async Task<IActionResult> GetByCustomer(int customerId)
    {
        var enrollments = await _enrollmentService
            .GetEnrollmentsByCustomerAsync(customerId);
        return Ok(enrollments);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] EnrollmentRequestDto request)
    {
        try
        {
            var enrollment = await _enrollmentService
                .CreateEnrollmentAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = enrollment.Id }, enrollment);
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
            await _enrollmentService.DeleteEnrollmentAsync(id);
            return Ok(new
            {
                message = "Enrollment deleted successfully!"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}