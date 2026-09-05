using HLChitFund.Application.DTOs.Report;

namespace HLChitFund.Application.Common.Interfaces;

public interface IReportService
{
    Task<IEnumerable<MonthlyCollectionDto>> GetMonthlyCollectionAsync(ReportFilterDto filter);
    Task<IEnumerable<ChitGroupSummaryDto>> GetChitGroupSummaryAsync();
    Task<CustomerStatementDto?> GetCustomerStatementAsync(int customerId);
    Task<IEnumerable<CommissionSummaryDto>> GetCommissionSummaryAsync(ReportFilterDto filter);
    Task<IEnumerable<WinnerSummaryDto>> GetWinnerSummaryAsync(ReportFilterDto filter);
}