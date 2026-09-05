using HLChitFund.Application.DTOs.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.Common.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentResponseDto>>
            GetAllEnrollmentsAsync();
        Task<IEnumerable<EnrollmentResponseDto>>
            GetEnrollmentsByChitGroupAsync(int chitGroupId);
        Task<IEnumerable<EnrollmentResponseDto>>
            GetEnrollmentsByCustomerAsync(int customerId);
        Task<EnrollmentResponseDto?> GetEnrollmentByIdAsync(int id);
        Task<EnrollmentResponseDto> CreateEnrollmentAsync(
            EnrollmentRequestDto request);
        Task<bool> DeleteEnrollmentAsync(int id);
    }

}
