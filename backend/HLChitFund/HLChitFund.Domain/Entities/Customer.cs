using HLChitFund.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string AadharNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation Property
        //public ICollection<Enrollment> Enrollments { get; set; }
        //    = new List<Enrollment>();
    }
}
