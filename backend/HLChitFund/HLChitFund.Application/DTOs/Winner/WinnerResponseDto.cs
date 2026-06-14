using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.DTOs.Winner;

public class WinnerResponseDto
{
    public int Id { get; set; }
    public int ChitGroupId { get; set; }
    public string ChitGroupName { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int EnrollmentId { get; set; }
    public int MonthNumber { get; set; }
    public decimal PrizeAmount { get; set; }
    public decimal CommissionDeducted { get; set; }
    public decimal NetAmount { get; set; }
    public DateTime WonDate { get; set; }
    public string SelectionType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
