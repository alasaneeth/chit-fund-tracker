namespace HLChitFund.Application.DTOs.Payment;

public class PaymentRequestDto
{
    public int EnrollmentId { get; set; }
    public int MonthNumber { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}