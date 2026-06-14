using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HLChitFund.Domain.Common;

namespace HLChitFund.Domain.Entities;

public class Winner : BaseEntity
{
    public int ChitGroupId { get; set; }
    public int CustomerId { get; set; }
    public int EnrollmentId { get; set; }
    public int MonthNumber { get; set; }
    public decimal PrizeAmount { get; set; }
    public decimal CommissionDeducted { get; set; }
    public decimal NetAmount { get; set; }
    public DateTime WonDate { get; set; } = DateTime.UtcNow;
    public string SelectionType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    // Navigation Properties
    public ChitGroup ChitGroup { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public Enrollment Enrollment { get; set; } = null!;
}