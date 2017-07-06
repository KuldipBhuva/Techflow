using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class QuotationModel
    {
        [Required(ErrorMessage = "Company Required")]
        public int? comp { get; set; }
        public int QID { get; set; }
        [Required(ErrorMessage = "Data Centre Required")]
        public Nullable<int> DataCenterOther { get; set; }
        [Required(ErrorMessage = "Data Centre Required")]        
        public Nullable<int> DataCenter { get; set; }
        [Required(ErrorMessage = "Cable Source Required")]
        public string CableSource { get; set; }
        [Required(ErrorMessage = "Cable Destination Required")]
        public string CableDest { get; set; }
        [Required(ErrorMessage = "Cable Type Required")]
        public string CableType { get; set; }
        [Required(ErrorMessage = "Cable Termination at Source Required")]
        public string TerminationSource { get; set; }
        [Required(ErrorMessage = "Cable Termination at Destination Required")]
        public string TerminationDest { get; set; }
        [Required(ErrorMessage = "Request Delivery Date Required")]
        public Nullable<System.DateTime> ReqDelDate { get; set; }
        public Nullable<bool> IsPremium { get; set; }
        public Nullable<bool> IsPatchPanel { get; set; }
        public string PatchPanel { get; set; }
        public Nullable<bool> IsMediaConverter { get; set; }
        public string MediaConverter { get; set; }
        public string Comments { get; set; }
        [Required(ErrorMessage = "Details of Service Required")]
        public string ServiceReq { get; set; }
        [Required(ErrorMessage = "Requested Date for Quote Required")]
        public Nullable<System.DateTime> DateForQuote { get; set; }
        [Required(ErrorMessage = "Requested Date for Service Required")]
        public Nullable<System.DateTime> DateForService { get; set; }
        public Nullable<bool> IsActive { get; set; }
        [Required(ErrorMessage = "User Required")]
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> TicketID { get; set; }
        public string Prefix { get; set; }

        public List<QuotationModel> ListQuo { get; set; }
        public UserModel UserDetails { get; set; }
        public DataCenterModel DCDetails { get; set; }
        public List<DataCenterModel> ListDC { get; set; }
        public CompanyModel CompDetails { get; set; }
        public List<CompanyModel> ListComp { get; set; }
        public List<UserModel> UserList { get; set; }
    }
}
