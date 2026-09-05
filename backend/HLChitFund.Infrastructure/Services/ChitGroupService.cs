using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.Common.Interfaces.Repositories;
using HLChitFund.Application.DTOs.ChitGroup;
using HLChitFund.Domain.Entities;
using HLChitFund.Domain.Enums;

namespace HLChitFund.Infrastructure.Services;

public class ChitGroupService : IChitGroupService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChitGroupService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ChitGroupResponseDto>>
        GetAllChitGroupsAsync()
    {
        var chitGroups = await _unitOfWork.ChitGroups.GetAllAsync();
        return chitGroups.Select(MapToResponse);
    }

    public async Task<ChitGroupResponseDto?>
        GetChitGroupByIdAsync(int id)
    {
        var chitGroup = await _unitOfWork.ChitGroups
            .GetByIdAsync(id);
        return chitGroup == null ? null : MapToResponse(chitGroup);
    }

    public async Task<ChitGroupResponseDto> CreateChitGroupAsync(
        ChitGroupRequestDto request)
    {
        var existing = await _unitOfWork.ChitGroups.FindAsync(
            c => c.Name == request.Name);

        if (existing.Any())
            throw new InvalidOperationException(
                "Chit Group name already exists!");

        var chitGroup = new ChitGroup
        {
            Name = request.Name,
            TotalAmount = request.TotalAmount,
            MonthlyAmount = request.MonthlyAmount,
            DurationMonths = request.DurationMonths,
            ChitType = Enum.Parse<ChitType>(request.ChitType),
            StartDate = request.StartDate,
            EndDate = request.StartDate.AddMonths(
                request.DurationMonths),
            MaxMembers = request.MaxMembers,
            CommissionRate = 3,
            IsActive = true
        };

        await _unitOfWork.ChitGroups.AddAsync(chitGroup);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(chitGroup);
    }

    public async Task<ChitGroupResponseDto> UpdateChitGroupAsync(
        int id, ChitGroupRequestDto request)
    {
        var chitGroup = await _unitOfWork.ChitGroups
            .GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                "Chit Group not found!");

        chitGroup.Name = request.Name;
        chitGroup.TotalAmount = request.TotalAmount;
        chitGroup.MonthlyAmount = request.MonthlyAmount;
        chitGroup.DurationMonths = request.DurationMonths;
        chitGroup.ChitType = Enum.Parse<ChitType>(request.ChitType);
        chitGroup.StartDate = request.StartDate;
        chitGroup.MaxMembers = request.MaxMembers;

        _unitOfWork.ChitGroups.Update(chitGroup);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(chitGroup);
    }

    public async Task<bool> DeleteChitGroupAsync(int id)
    {
        var chitGroup = await _unitOfWork.ChitGroups
            .GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                "Chit Group not found!");

        _unitOfWork.ChitGroups.Delete(chitGroup);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ToggleChitGroupStatusAsync(int id)
    {
        var chitGroup = await _unitOfWork.ChitGroups
            .GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                "Chit Group not found!");

        chitGroup.IsActive = !chitGroup.IsActive;
        _unitOfWork.ChitGroups.Update(chitGroup);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static ChitGroupResponseDto MapToResponse(
        ChitGroup chitGroup) => new()
        {
            Id = chitGroup.Id,
            Name = chitGroup.Name,
            TotalAmount = chitGroup.TotalAmount,
            MonthlyAmount = chitGroup.MonthlyAmount,
            DurationMonths = chitGroup.DurationMonths,
            ChitType = chitGroup.ChitType.ToString(),
            StartDate = chitGroup.StartDate,
            EndDate = chitGroup.EndDate,
            CommissionRate = chitGroup.CommissionRate,
            IsActive = chitGroup.IsActive,
            MaxMembers = chitGroup.MaxMembers,
            CurrentMembers = chitGroup.Enrollments?.Count ?? 0
        };
}