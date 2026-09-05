using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Winner;
using HLChitFund.Domain.Entities;

namespace HLChitFund.Infrastructure.Services;

public class WinnerService : IWinnerService
{
    private readonly IUnitOfWork _unitOfWork;

    public WinnerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<WinnerResponseDto>> GetAllWinnersAsync()
    {
        var winners = await _unitOfWork.Winners
            .GetAllAsync(
                w => w.Customer,
                w => w.ChitGroup,
                w => w.Enrollment
            );
        return winners.Select(MapToResponse);
    }

    public async Task<IEnumerable<WinnerResponseDto>>
        GetWinnersByChitGroupAsync(int chitGroupId)
    {
        var winners = await _unitOfWork.Winners
            .FindAsync(w => w.ChitGroupId == chitGroupId);
        return winners.Select(MapToResponse);
    }

    public async Task<WinnerResponseDto?> GetWinnerByIdAsync(int id)
    {
        var winner = await _unitOfWork.Winners.GetByIdAsync(id);
        return winner == null ? null : MapToResponse(winner);
    }

    public async Task<WinnerResponseDto> SelectWinnerAsync(
        WinnerRequestDto request)
    {
        var chitGroup = await _unitOfWork.ChitGroups
            .GetByIdAsync(request.ChitGroupId)
            ?? throw new KeyNotFoundException(
                "Chit Group not found!");

        var customer = await _unitOfWork.Customers
            .GetByIdAsync(request.CustomerId)
            ?? throw new KeyNotFoundException(
                "Customer not found!");

        var enrollment = await _unitOfWork.Enrollments
            .GetByIdAsync(request.EnrollmentId)
            ?? throw new KeyNotFoundException(
                "Enrollment not found!");

        var alreadyWon = await _unitOfWork.Winners.FindAsync(
            w => w.ChitGroupId == request.ChitGroupId &&
                 w.MonthNumber == request.MonthNumber);

        if (alreadyWon.Any())
            throw new InvalidOperationException(
                "Winner already selected for this month!");

        var commissionRate = chitGroup.CommissionRate;
        var commissionAmount = request.PrizeAmount *
            commissionRate / 100;
        var netAmount = request.PrizeAmount - commissionAmount;

        var winner = new Winner
        {
            ChitGroupId = request.ChitGroupId,
            CustomerId = request.CustomerId,
            EnrollmentId = request.EnrollmentId,
            MonthNumber = request.MonthNumber,
            PrizeAmount = request.PrizeAmount,
            CommissionDeducted = commissionAmount,
            NetAmount = netAmount,
            WonDate = DateTime.UtcNow,
            SelectionType = request.SelectionType,
            Notes = request.Notes
        };

        await _unitOfWork.Winners.AddAsync(winner);
        await _unitOfWork.SaveChangesAsync();

        enrollment.HasWon = true;
        _unitOfWork.Enrollments.Update(enrollment);

        var commission = new Commission
        {
            ChitGroupId = request.ChitGroupId,
            WinnerId = winner.Id,
            MonthNumber = request.MonthNumber,
            TotalAmount = request.PrizeAmount,
            CommissionRate = commissionRate,
            CommissionAmount = commissionAmount,
            RecordedDate = DateTime.UtcNow,
            Notes = $"Commission for Month {request.MonthNumber}"
        };

        await _unitOfWork.Commissions.AddAsync(commission);
        await _unitOfWork.SaveChangesAsync();

        winner.ChitGroup = chitGroup;
        winner.Customer = customer;
        winner.Enrollment = enrollment;

        return MapToResponse(winner);
    }

    public async Task<bool> DeleteWinnerAsync(int id)
    {
        var winner = await _unitOfWork.Winners.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                "Winner not found!");

        _unitOfWork.Winners.Delete(winner);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static WinnerResponseDto MapToResponse(
        Winner winner) => new()
        {
            Id = winner.Id,
            ChitGroupId = winner.ChitGroupId,
            ChitGroupName = winner.ChitGroup?.Name ?? string.Empty,
            CustomerId = winner.CustomerId,
            CustomerName = winner.Customer?.FullName ?? string.Empty,
            EnrollmentId = winner.EnrollmentId,
            MonthNumber = winner.MonthNumber,
            PrizeAmount = winner.PrizeAmount,
            CommissionDeducted = winner.CommissionDeducted,
            NetAmount = winner.NetAmount,
            WonDate = winner.WonDate,
            SelectionType = winner.SelectionType,
            Notes = winner.Notes
        };
}
