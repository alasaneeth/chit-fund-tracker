using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Report;
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
    public async Task<IActionResult> GetMonthlyCollection(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int? chitGroupId)
    {
        var filter = new ReportFilterDto
        {
            FromDate = fromDate,
            ToDate = toDate,
            ChitGroupId = chitGroupId
        };
        var result = await _reportService.GetMonthlyCollectionAsync(filter);
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
    public async Task<IActionResult> GetCommissionSummary(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int? chitGroupId)
    {
        var filter = new ReportFilterDto
        {
            FromDate = fromDate,
            ToDate = toDate,
            ChitGroupId = chitGroupId
        };
        var result = await _reportService.GetCommissionSummaryAsync(filter);
        return Ok(result);
    }

    [HttpGet("winner-summary")]
    public async Task<IActionResult> GetWinnerSummary(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int? chitGroupId)
    {
        var filter = new ReportFilterDto
        {
            FromDate = fromDate,
            ToDate = toDate,
            ChitGroupId = chitGroupId
        };
        var result = await _reportService.GetWinnerSummaryAsync(filter);
        return Ok(result);
    }
}