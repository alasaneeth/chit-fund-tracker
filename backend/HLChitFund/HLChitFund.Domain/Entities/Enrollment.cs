using HLChitFund.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public int CustomerId { get; set; }
        public int ChitGroupId { get; set; }
        public int SlotNumber { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public bool HasWon { get; set; } = false;

        // Navigation Properties
        public Customer Customer { get; set; } = null!;
        public ChitGroup ChitGroup { get; set; } = null!;
    }
}
