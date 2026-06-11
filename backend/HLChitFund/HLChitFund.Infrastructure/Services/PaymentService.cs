using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Payment;
using HLChitFund.Domain.Entities;

namespace HLChitFund.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PaymentResponseDto>>
        GetAllPaymentsAsync()
    {
        var payments = await _unitOfWork.Payments.GetAllAsync();
        return payments.Select(MapToResponse);
    }

    public async Task<IEnumerable<PaymentResponseDto>>
        GetPaymentsByEnrollmentAsync(int enrollmentId)
    {
        var payments = await _unitOfWork.Payments
            .FindAsync(p => p.EnrollmentId == enrollmentId);
        return payments.Select(MapToResponse);
    }

    public async Task<IEnumerable<PaymentResponseDto>>
        GetPaymentsByChitGroupAsync(int chitGroupId)
    {
        var enrollments = await _unitOfWork.Enrollments
            .FindAsync(e => e.ChitGroupId == chitGroupId);

        var enrollmentIds = enrollments.Select(e => e.Id).ToList();

        var payments = await _unitOfWork.Payments
            .FindAsync(p => enrollmentIds.Contains(p.EnrollmentId));

        return payments.Select(MapToResponse);
    }

    public async Task<PaymentResponseDto?> GetPaymentByIdAsync(int id)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id);
        return payment == null ? null : MapToResponse(payment);
    }

    public async Task<PaymentResponseDto> CollectPaymentAsync(
        PaymentRequestDto request)
    {
        // Enrollment check பண்ணு
        var enrollment = await _unitOfWork.Enrollments
            .GetByIdAsync(request.EnrollmentId)
            ?? throw new KeyNotFoundException(
                "Enrollment not found!");

        // Already paid check பண்ணு
        var existing = await _unitOfWork.Payments.FindAsync(
            p => p.EnrollmentId == request.EnrollmentId &&
                 p.MonthNumber == request.MonthNumber);

        if (existing.Any())
            throw new InvalidOperationException(
                "Payment already collected for this month!");

        // ChitGroup எடு
        var chitGroup = await _unitOfWork.ChitGroups
            .GetByIdAsync(enrollment.ChitGroupId)
            ?? throw new KeyNotFoundException(
                "Chit Group not found!");

        // Customer எடு
        var customer = await _unitOfWork.Customers
            .GetByIdAsync(enrollment.CustomerId)
            ?? throw new KeyNotFoundException(
                "Customer not found!");

        // Receipt Number Generate பண்ணு
        var receiptNumber = $"RCP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";

        var payment = new Payment
        {
            EnrollmentId = request.EnrollmentId,
            MonthNumber = request.MonthNumber,
            AmountPaid = request.AmountPaid,
            PaidDate = DateTime.UtcNow,
            ReceiptNumber = receiptNumber,
            PaymentMode = request.PaymentMode,
            Notes = request.Notes,
            IsLate = false
        };

        await _unitOfWork.Payments.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();

        payment.Enrollment = enrollment;
        enrollment.Customer = customer;
        enrollment.ChitGroup = chitGroup;

        return MapToResponse(payment);
    }

    public async Task<bool> DeletePaymentAsync(int id)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                "Payment not found!");

        _unitOfWork.Payments.Delete(payment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static PaymentResponseDto MapToResponse(
        Payment payment) => new()
        {
            Id = payment.Id,
            EnrollmentId = payment.EnrollmentId,
            CustomerName = payment.Enrollment?.Customer?.FullName
            ?? string.Empty,
            ChitGroupName = payment.Enrollment?.ChitGroup?.Name
            ?? string.Empty,
            MonthNumber = payment.MonthNumber,
            AmountPaid = payment.AmountPaid,
            PaidDate = payment.PaidDate,
            ReceiptNumber = payment.ReceiptNumber,
            PaymentMode = payment.PaymentMode,
            Notes = payment.Notes,
            IsLate = payment.IsLate
        };
}