using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.Common.Interfaces.Repositories;
using HLChitFund.Application.DTOs.ChitGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HLChitFund.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChitGroupController : ControllerBase
{
    private readonly IChitGroupService _chitGroupService;

    public ChitGroupController(IChitGroupService chitGroupService)
    {
        _chitGroupService = chitGroupService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetAll()
    {
        var chitGroups = await _chitGroupService
            .GetAllChitGroupsAsync();
        return Ok(chitGroups);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> GetById(int id)
    {
        var chitGroup = await _chitGroupService
            .GetChitGroupByIdAsync(id);

        if (chitGroup == null)
            return NotFound(new { message = "Chit Group not found!" });

        return Ok(chitGroup);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] ChitGroupRequestDto request)
    {
        try
        {
            var chitGroup = await _chitGroupService
                .CreateChitGroupAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = chitGroup.Id }, chitGroup);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id, [FromBody] ChitGroupRequestDto request)
    {
        try
        {
            var chitGroup = await _chitGroupService
                .UpdateChitGroupAsync(id, request);
            return Ok(chitGroup);
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
            await _chitGroupService.DeleteChitGroupAsync(id);
            return Ok(new
            {
                message = "Chit Group deleted successfully!"
            });
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
            await _chitGroupService.ToggleChitGroupStatusAsync(id);
            return Ok(new
            {
                message = "Chit Group status updated!"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}