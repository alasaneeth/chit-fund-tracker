using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HLChitFund.Application.DTOs.Report;

namespace HLChitFund.Application.Common.Interfaces;

public interface IReportService
{
    Task<IEnumerable<MonthlyCollectionDto>> GetMonthlyCollectionAsync();
    Task<IEnumerable<ChitGroupSummaryDto>> GetChitGroupSummaryAsync();
    Task<CustomerStatementDto?> GetCustomerStatementAsync(int customerId);
    Task<IEnumerable<CommissionSummaryDto>> GetCommissionSummaryAsync();
    Task<IEnumerable<WinnerSummaryDto>> GetWinnerSummaryAsync();
}