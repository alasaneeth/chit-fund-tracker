using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.DTOs.Enrollment
{
    public class EnrollmentResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int ChitGroupId { get; set; }
        public string ChitGroupName { get; set; } = string.Empty;
        public int SlotNumber { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }
        public bool HasWon { get; set; }
    }
}
