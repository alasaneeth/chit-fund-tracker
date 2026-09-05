using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Customer;
using HLChitFund.Domain.Entities;

namespace HLChitFund.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CustomerResponseDto>>
        GetAllCustomersAsync()
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();
        return customers.Select(MapToResponse);
    }

    public async Task<CustomerResponseDto?>
        GetCustomerByIdAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        return customer == null ? null : MapToResponse(customer);
    }

    public async Task<CustomerResponseDto> CreateCustomerAsync(
        CustomerRequestDto request)
    {
        var existing = await _unitOfWork.Customers.FindAsync(
            c => c.Email == request.Email);

        if (existing.Any())
            throw new InvalidOperationException(
                "Email already exists!");

        var customer = new Customer
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            AadharNumber = request.AadharNumber,
            DateOfBirth = request.DateOfBirth,
            JoinDate = DateTime.UtcNow,
            IsActive = true
        };

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(customer);
    }

    public async Task<CustomerResponseDto> UpdateCustomerAsync(
        int id, CustomerRequestDto request)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                "Customer not found!");

        customer.FullName = request.FullName;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.Address = request.Address;
        customer.AadharNumber = request.AadharNumber;
        customer.DateOfBirth = request.DateOfBirth;

        _unitOfWork.Customers.Update(customer);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(customer);
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                "Customer not found!");

        _unitOfWork.Customers.Delete(customer);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ToggleCustomerStatusAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                "Customer not found!");

        customer.IsActive = !customer.IsActive;
        _unitOfWork.Customers.Update(customer);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static CustomerResponseDto MapToResponse(
        Customer customer) => new()
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            AadharNumber = customer.AadharNumber,
            DateOfBirth = customer.DateOfBirth,
            JoinDate = customer.JoinDate,
            IsActive = customer.IsActive
        };
}