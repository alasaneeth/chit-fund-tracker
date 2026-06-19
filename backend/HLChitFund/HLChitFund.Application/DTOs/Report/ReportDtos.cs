using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.DTOs.Report;

public class MonthlyCollectionDto
{
    public int MonthNumber { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal TotalCollected { get; set; }
    public int TotalPayments { get; set; }
    public int LatePayments { get; set; }
}

public class ChitGroupSummaryDto
{
    public int ChitGroupId { get; set; }
    public string ChitGroupName { get; set; } = string.Empty;
    public string ChitType { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int TotalMembers { get; set; }
    public int EnrolledMembers { get; set; }
    public decimal TotalCollected { get; set; }
    public int TotalWinners { get; set; }
    public decimal TotalCommission { get; set; }
}

public class CustomerStatementDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public List<CustomerPaymentDto> Payments { get; set; } = new();
    public decimal TotalPaid { get; set; }
    public bool HasWon { get; set; }
    public int TotalEnrollments { get; set; }
}

public class CustomerPaymentDto
{
    public int EnrollmentId { get; set; }
    public string ChitGroupName { get; set; } = string.Empty;
    public int MonthNumber { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaidDate { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public bool IsLate { get; set; }
}

public class CommissionSummaryDto
{
    public int ChitGroupId { get; set; }
    public string ChitGroupName { get; set; } = string.Empty;
    public int MonthNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal CommissionAmount { get; set; }
    public DateTime RecordedDate { get; set; }
}

public class WinnerSummaryDto
{
    public int ChitGroupId { get; set; }
    public string ChitGroupName { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public int MonthNumber { get; set; }
    public decimal PrizeAmount { get; set; }
    public decimal CommissionDeducted { get; set; }
    public decimal NetAmount { get; set; }
    public string SelectionType { get; set; } = string.Empty;
    public DateTime WonDate { get; set; }
}
