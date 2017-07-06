using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class PowerUsageModel
    {
        [Required(ErrorMessage = "Year Required")]
        public int year { get; set; }
        [Required(ErrorMessage = "Month Required")]
        public int month { get; set; }
        public int PUID { get; set; }
        public Nullable<int> CompID { get; set; }
        public string DataSuite { get; set; }
        public string RackID { get; set; }
        public string PowerFeeds { get; set; }
        public string AmpereRating { get; set; }
        public Nullable<decimal> PowerContracted { get; set; }
        public Nullable<decimal> PowerUsed { get; set; }
        public string OverUnder { get; set; }
        public Nullable<System.DateTime> LastReading { get; set; }

        public int TTyear { get; set; }
        public string TTmonth { get; set; }
        public decimal TotalPC { get; set; }
        public decimal TotalPU { get; set; }
        

        public List<PowerUsageModel> ListPowerData { get; set; }
        public CompanyModel CompDetails { get; set; }
        public List<CompanyModel> ListComp { get; set; }
        public List<PowerUsageModel> ListTotalTickets { get; set; }
    }

}
