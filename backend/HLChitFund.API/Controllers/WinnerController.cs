using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Winner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HLChitFund.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WinnerController : ControllerBase
{
    private readonly IWinnerService _winnerService;

    public WinnerController(IWinnerService winnerService)
    {
        _winnerService = winnerService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetAll()
    {
        var winners = await _winnerService.GetAllWinnersAsync();
        return Ok(winners);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetById(int id)
    {
        var winner = await _winnerService.GetWinnerByIdAsync(id);

        if (winner == null)
            return NotFound(new { message = "Winner not found!" });

        return Ok(winner);
    }

    [HttpGet("chit-group/{chitGroupId}")]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetByChitGroup(int chitGroupId)
    {
        var winners = await _winnerService
            .GetWinnersByChitGroupAsync(chitGroupId);
        return Ok(winners);
    }

    [HttpPost("select")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SelectWinner(
        [FromBody] WinnerRequestDto request)
    {
        try
        {
            var winner = await _winnerService
                .SelectWinnerAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = winner.Id }, winner);
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
            await _winnerService.DeleteWinnerAsync(id);
            return Ok(new { message = "Winner deleted successfully!" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}