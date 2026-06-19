using HLChitFund.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HLChitFund.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("monthly-collection")]
    public async Task<IActionResult> GetMonthlyCollection()
    {
        var result = await _reportService.GetMonthlyCollectionAsync();
        return Ok(result);
    }

    [HttpGet("chit-group-summary")]
    public async Task<IActionResult> GetChitGroupSummary()
    {
        var result = await _reportService.GetChitGroupSummaryAsync();
        return Ok(result);
    }

    [HttpGet("customer-statement/{customerId}")]
    public async Task<IActionResult> GetCustomerStatement(int customerId)
    {
        var result = await _reportService
            .GetCustomerStatementAsync(customerId);
        if (result == null)
            return NotFound("Customer not found!");
        return Ok(result);
    }

    [HttpGet("commission-summary")]
    public async Task<IActionResult> GetCommissionSummary()
    {
        var result = await _reportService.GetCommissionSummaryAsync();
        return Ok(result);
    }

    [HttpGet("winner-summary")]
    public async Task<IActionResult> GetWinnerSummary()
    {
        var result = await _reportService.GetWinnerSummaryAsync();
        return Ok(result);
    }
}