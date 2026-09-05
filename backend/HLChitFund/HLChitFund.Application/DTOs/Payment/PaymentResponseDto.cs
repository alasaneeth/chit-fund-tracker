namespace HLChitFund.Application.DTOs.Payment;

public class PaymentResponseDto
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string ChitGroupName { get; set; } = string.Empty;
    public int MonthNumber { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaidDate { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public string PaymentMode { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool IsLate { get; set; }
}