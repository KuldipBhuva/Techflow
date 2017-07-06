using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TicketTranModel
    {
        public bool response { get; set; }
        public int TTranID { get; set; }
        public Nullable<int> TicketID { get; set; }
        public Nullable<int> AssignTo { get; set; }
        public string Comment { get; set; }
        public string FileName { get; set; }
        public string FileURL { get; set; }
        public Nullable<int> ReplayBy { get; set; }
        public Nullable<System.DateTime> ReplayDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Prefix { get; set; }
        

        public UserModel UserDetails { get; set; }
    }

   
}
