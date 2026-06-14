using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.DTOs.Winner;

public class WinnerRequestDto
{
    public int ChitGroupId { get; set; }
    public int CustomerId { get; set; }
    public int EnrollmentId { get; set; }
    public int MonthNumber { get; set; }
    public decimal PrizeAmount { get; set; }
    public string SelectionType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
