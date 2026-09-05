using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.DTOs.Customer
{
    public class CustomerResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string AadharNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }
    }
}
