using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TicketAttachmentModel
    {
        public int TAID { get; set; }
        public Nullable<int> TTranID { get; set; }
        public string FileName { get; set; }
        public string FileURL { get; set; }
        public Nullable<int> ReplayBy { get; set; }
        public Nullable<System.DateTime> ReplayDate { get; set; }
    }
}
