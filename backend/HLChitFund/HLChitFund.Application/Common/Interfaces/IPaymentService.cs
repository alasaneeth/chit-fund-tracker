using HLChitFund.Application.DTOs.Payment;

namespace HLChitFund.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync();
    Task<IEnumerable<PaymentResponseDto>>
        GetPaymentsByEnrollmentAsync(int enrollmentId);
    Task<IEnumerable<PaymentResponseDto>>
        GetPaymentsByChitGroupAsync(int chitGroupId);
    Task<PaymentResponseDto?> GetPaymentByIdAsync(int id);
    Task<PaymentResponseDto> CollectPaymentAsync(
        PaymentRequestDto request);
    Task<bool> DeletePaymentAsync(int id);
}