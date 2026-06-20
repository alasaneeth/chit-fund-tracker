using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Report;

namespace HLChitFund.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MonthlyCollectionDto>> GetMonthlyCollectionAsync(
    ReportFilterDto filter)
    {
        var payments = await _unitOfWork.Payments
            .GetAllAsync(p => p.Enrollment);

        var filtered = payments.AsEnumerable();

        if (filter.FromDate.HasValue)
            filtered = filtered.Where(p => p.PaidDate >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            filtered = filtered.Where(p => p.PaidDate <= filter.ToDate.Value);

        if (filter.ChitGroupId.HasValue)
            filtered = filtered.Where(p => p.Enrollment.ChitGroupId == filter.ChitGroupId.Value);

        var grouped = filtered
            .GroupBy(p => new { p.PaidDate.Year, p.PaidDate.Month })
            .Select(g => new MonthlyCollectionDto
            {
                Year = g.Key.Year,
                MonthNumber = g.Key.Month,
                MonthName = new DateTime(g.Key.Year, g.Key.Month, 1)
                    .ToString("MMMM"),
                TotalCollected = g.Sum(p => p.AmountPaid),
                TotalPayments = g.Count(),
                LatePayments = g.Count(p => p.IsLate)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.MonthNumber);

        return grouped;
    }


    public async Task<IEnumerable<ChitGroupSummaryDto>> GetChitGroupSummaryAsync()
    {
        var chitGroups = await _unitOfWork.ChitGroups.GetAllAsync();
        var enrollments = await _unitOfWork.Enrollments.GetAllAsync();
        var payments = await _unitOfWork.Payments
            .GetAllAsync(p => p.Enrollment);
        var winners = await _unitOfWork.Winners.GetAllAsync();
        var commissions = await _unitOfWork.Commissions.GetAllAsync();

        var result = chitGroups.Select(g => new ChitGroupSummaryDto
        {
            ChitGroupId = g.Id,
            ChitGroupName = g.Name,
            ChitType = g.ChitType.ToString(),
            TotalAmount = g.TotalAmount,
            TotalMembers = g.MaxMembers,
            EnrolledMembers = enrollments.Count(e => e.ChitGroupId == g.Id),
            TotalCollected = payments
                .Where(p => p.Enrollment.ChitGroupId == g.Id)
                .Sum(p => p.AmountPaid),
            TotalWinners = winners.Count(w => w.ChitGroupId == g.Id),
            TotalCommission = commissions
                .Where(c => c.ChitGroupId == g.Id)
                .Sum(c => c.CommissionAmount)
        });

        return result;
    }

    public async Task<CustomerStatementDto?> GetCustomerStatementAsync(int customerId)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
        if (customer == null) return null;

        var enrollments = await _unitOfWork.Enrollments
            .GetAllAsync(e => e.ChitGroup);
        var customerEnrollments = enrollments
            .Where(e => e.CustomerId == customerId).ToList();

        var payments = await _unitOfWork.Payments
            .GetAllAsync(p => p.Enrollment, p => p.Enrollment.ChitGroup);
        var customerPayments = payments
            .Where(p => p.Enrollment.CustomerId == customerId).ToList();

        var winners = await _unitOfWork.Winners
            .FindAsync(w => w.CustomerId == customerId);

        var paymentDtos = customerPayments.Select(p => new CustomerPaymentDto
        {
            EnrollmentId = p.EnrollmentId,
            ChitGroupName = p.Enrollment?.ChitGroup?.Name ?? string.Empty,
            MonthNumber = p.MonthNumber,
            AmountPaid = p.AmountPaid,
            PaidDate = p.PaidDate,
            ReceiptNumber = p.ReceiptNumber,
            IsLate = p.IsLate
        }).ToList();

        return new CustomerStatementDto
        {
            CustomerId = customer.Id,
            CustomerName = customer.FullName,
            Email = customer.Email,
            Phone = customer.Phone,
            Payments = paymentDtos,
            TotalPaid = customerPayments.Sum(p => p.AmountPaid),
            HasWon = winners.Any(),
            TotalEnrollments = customerEnrollments.Count
        };
    }

    public async Task<IEnumerable<CommissionSummaryDto>> GetCommissionSummaryAsync(
     ReportFilterDto filter)
    {
        var commissions = await _unitOfWork.Commissions
            .GetAllAsync(c => c.ChitGroup);

        var filtered = commissions.AsEnumerable();

        if (filter.FromDate.HasValue)
            filtered = filtered.Where(c => c.RecordedDate >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            filtered = filtered.Where(c => c.RecordedDate <= filter.ToDate.Value);

        if (filter.ChitGroupId.HasValue)
            filtered = filtered.Where(c => c.ChitGroupId == filter.ChitGroupId.Value);

        return filtered.Select(c => new CommissionSummaryDto
        {
            ChitGroupId = c.ChitGroupId,
            ChitGroupName = c.ChitGroup?.Name ?? string.Empty,
            MonthNumber = c.MonthNumber,
            TotalAmount = c.TotalAmount,
            CommissionRate = c.CommissionRate,
            CommissionAmount = c.CommissionAmount,
            RecordedDate = c.RecordedDate
        });
    }
    public async Task<IEnumerable<WinnerSummaryDto>> GetWinnerSummaryAsync(
     ReportFilterDto filter)
    {
        var winners = await _unitOfWork.Winners
            .GetAllAsync(w => w.Customer, w => w.ChitGroup);

        var filtered = winners.AsEnumerable();

        if (filter.FromDate.HasValue)
            filtered = filtered.Where(w => w.WonDate >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            filtered = filtered.Where(w => w.WonDate <= filter.ToDate.Value);

        if (filter.ChitGroupId.HasValue)
            filtered = filtered.Where(w => w.ChitGroupId == filter.ChitGroupId.Value);

        return filtered.Select(w => new WinnerSummaryDto
        {
            ChitGroupId = w.ChitGroupId,
            ChitGroupName = w.ChitGroup?.Name ?? string.Empty,
            CustomerName = w.Customer?.FullName ?? string.Empty,
            MonthNumber = w.MonthNumber,
            PrizeAmount = w.PrizeAmount,
            CommissionDeducted = w.CommissionDeducted,
            NetAmount = w.NetAmount,
            SelectionType = w.SelectionType,
            WonDate = w.WonDate
        });
    }
}