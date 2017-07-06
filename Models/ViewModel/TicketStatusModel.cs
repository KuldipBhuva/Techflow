using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TicketStatusModel
    {
        public int TicketStatusID { get; set; }
        public string TicketStatus { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
