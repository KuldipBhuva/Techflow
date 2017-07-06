using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TicketTypeModel
    {
        public int TicketTypeID { get; set; }
        public string TicketType { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
