using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.DTOs.ChitGroup
{
    public class ChitGroupRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal MonthlyAmount { get; set; }
        public int DurationMonths { get; set; }
        public string ChitType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int MaxMembers { get; set; }
    }
}
