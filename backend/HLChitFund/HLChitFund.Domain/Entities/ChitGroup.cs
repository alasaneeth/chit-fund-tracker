using HLChitFund.Domain.Common;
using HLChitFund.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Domain.Entities
{
    public class ChitGroup : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal MonthlyAmount { get; set; }
        public int DurationMonths { get; set; }
        public ChitType ChitType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal CommissionRate { get; set; } = 3;
        public bool IsActive { get; set; } = true;
        public int MaxMembers { get; set; }

        // Navigation Property
        public ICollection<Enrollment> Enrollments { get; set; }
            = new List<Enrollment>();
    }
}
