using HLChitFund.Domain.Common;
using System;

namespace HLChitFund.Domain.Entities;

public class Payment : BaseEntity
{
    public int EnrollmentId { get; set; }
    public int MonthNumber { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaidDate { get; set; } = DateTime.UtcNow;
    public string ReceiptNumber { get; set; } = string.Empty;
    public string PaymentMode { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool IsLate { get; set; } = false;

    // Navigation Property
    public Enrollment Enrollment { get; set; } = null!;
}