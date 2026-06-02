using HLChitFund.Application.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.Common.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerResponseDto>> GetAllCustomersAsync();
        Task<CustomerResponseDto?> GetCustomerByIdAsync(int id);
        Task<CustomerResponseDto> CreateCustomerAsync(
            CustomerRequestDto request);
        Task<CustomerResponseDto> UpdateCustomerAsync(
            int id, CustomerRequestDto request);
        Task<bool> DeleteCustomerAsync(int id);
        Task<bool> ToggleCustomerStatusAsync(int id);
    }
}
