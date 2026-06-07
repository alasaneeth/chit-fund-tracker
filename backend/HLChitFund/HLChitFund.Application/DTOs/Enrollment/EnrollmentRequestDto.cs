using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.DTOs.Enrollment
{
    public class EnrollmentRequestDto
    {
        public int CustomerId { get; set; }
        public int ChitGroupId { get; set; }
        public int SlotNumber { get; set; }
    }
}
