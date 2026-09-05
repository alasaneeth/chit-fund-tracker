using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HLChitFund.Domain.Common;

namespace HLChitFund.Domain.Entities;

public class Commission : BaseEntity
{
    public int ChitGroupId { get; set; }
    public int WinnerId { get; set; }
    public int MonthNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal CommissionAmount { get; set; }
    public DateTime RecordedDate { get; set; } = DateTime.UtcNow;
    public string Notes { get; set; } = string.Empty;

    // Navigation Properties
    public ChitGroup ChitGroup { get; set; } = null!;
    public Winner Winner { get; set; } = null!;
}
