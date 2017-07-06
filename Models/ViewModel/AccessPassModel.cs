using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class AccessPassModel
    {
        [Required(ErrorMessage = "Year Required")]
        public int year { get; set; }
        [Required(ErrorMessage = "Month Required")]
        public int month { get; set; }
        public int APID { get; set; }
        public Nullable<int> CompID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<decimal> AccessPassID { get; set; }
        public Nullable<System.DateTime> SwipeDate { get; set; }
        public Nullable<System.DateTime> SwipeTime { get; set; }
        public string DoorID { get; set; }

        public CompanyModel CompDetails { get; set; }
        public List<CompanyModel> ListComp { get; set; }
        public List<AccessPassModel> ListAP { get; set; }
    }
}
