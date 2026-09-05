using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.DTOs.ChitGroup
{
    public class ChitGroupResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal MonthlyAmount { get; set; }
        public int DurationMonths { get; set; }
        public string ChitType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal CommissionRate { get; set; }
        public bool IsActive { get; set; }
        public int MaxMembers { get; set; }
        public int CurrentMembers { get; set; }
    }
}
