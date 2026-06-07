using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Enrollment;
using HLChitFund.Domain.Entities;

namespace HLChitFund.Infrastructure.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public EnrollmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EnrollmentResponseDto>>
        GetAllEnrollmentsAsync()
    {
        var enrollments = await _unitOfWork.Enrollments
            .GetAllAsync();
        return enrollments.Select(MapToResponse);
    }

    public async Task<IEnumerable<EnrollmentResponseDto>>
        GetEnrollmentsByChitGroupAsync(int chitGroupId)
    {
        var enrollments = await _unitOfWork.Enrollments
            .FindAsync(e => e.ChitGroupId == chitGroupId);
        return enrollments.Select(MapToResponse);
    }

    public async Task<IEnumerable<EnrollmentResponseDto>>
        GetEnrollmentsByCustomerAsync(int customerId)
    {
        var enrollments = await _unitOfWork.Enrollments
            .FindAsync(e => e.CustomerId == customerId);
        return enrollments.Select(MapToResponse);
    }

    public async Task<EnrollmentResponseDto?>
        GetEnrollmentByIdAsync(int id)
    {
        var enrollment = await _unitOfWork.Enrollments
            .GetByIdAsync(id);
        return enrollment == null ? null : MapToResponse(enrollment);
    }

    public async Task<EnrollmentResponseDto> CreateEnrollmentAsync(
        EnrollmentRequestDto request)
    {
        // Customer இருக்கானு check பண்ணு
        var customer = await _unitOfWork.Customers
            .GetByIdAsync(request.CustomerId)
            ?? throw new KeyNotFoundException(
                "Customer not found!");

        // ChitGroup இருக்கானு check பண்ணு
        var chitGroup = await _unitOfWork.ChitGroups
            .GetByIdAsync(request.ChitGroupId)
            ?? throw new KeyNotFoundException(
                "Chit Group not found!");

        // Already enrolled check பண்ணு
        var existing = await _unitOfWork.Enrollments.FindAsync(
            e => e.CustomerId == request.CustomerId &&
                 e.ChitGroupId == request.ChitGroupId);

        if (existing.Any())
            throw new InvalidOperationException(
                "Customer already enrolled in this Chit Group!");

        // Max members check
        var currentMembers = await _unitOfWork.Enrollments
            .FindAsync(e => e.ChitGroupId == request.ChitGroupId);

        if (currentMembers.Count() >= chitGroup.MaxMembers)
            throw new InvalidOperationException(
                "Chit Group is full!");

        // Slot number already taken check 
        var slotTaken = await _unitOfWork.Enrollments.FindAsync(
            e => e.ChitGroupId == request.ChitGroupId &&
                 e.SlotNumber == request.SlotNumber);

        if (slotTaken.Any())
            throw new InvalidOperationException(
                "Slot number already taken!");

        var enrollment = new Enrollment
        {
            CustomerId = request.CustomerId,
            ChitGroupId = request.ChitGroupId,
            SlotNumber = request.SlotNumber,
            JoinDate = DateTime.UtcNow,
            IsActive = true,
            HasWon = false
        };

        await _unitOfWork.Enrollments.AddAsync(enrollment);
        await _unitOfWork.SaveChangesAsync();

        enrollment.Customer = customer;
        enrollment.ChitGroup = chitGroup;

        return MapToResponse(enrollment);
    }

    public async Task<bool> DeleteEnrollmentAsync(int id)
    {
        var enrollment = await _unitOfWork.Enrollments
            .GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                "Enrollment not found!");

        _unitOfWork.Enrollments.Delete(enrollment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static EnrollmentResponseDto MapToResponse(
        Enrollment enrollment) => new()
        {
            Id = enrollment.Id,
            CustomerId = enrollment.CustomerId,
            CustomerName = enrollment.Customer?.FullName ?? string.Empty,
            ChitGroupId = enrollment.ChitGroupId,
            ChitGroupName = enrollment.ChitGroup?.Name ?? string.Empty,
            SlotNumber = enrollment.SlotNumber,
            JoinDate = enrollment.JoinDate,
            IsActive = enrollment.IsActive,
            HasWon = enrollment.HasWon
        };
}