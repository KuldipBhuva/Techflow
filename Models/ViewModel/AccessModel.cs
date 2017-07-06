using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class AccessModel
    {
        public int BAID { get; set; }
        [Required(ErrorMessage = "Company Required")]
        public int? comp { get; set; }
        [Required(ErrorMessage = "Data Centre Required")]
        public Nullable<int> DataCenter { get; set; }
        [Required(ErrorMessage = "Visitor-1 First Name Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Visitor-1 Last Name Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Company Name Required")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Start Date/Time Required")]
        public Nullable<System.DateTime> StDateTime { get; set; }        
        public Nullable<System.TimeSpan> StTime { get; set; }
        [Required(ErrorMessage = "End Date/Time Required")]
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        [Required(ErrorMessage = "Reason Required")]
        public string Reason { get; set; }
        public Nullable<bool> IsActive { get; set; }
        [Required(ErrorMessage = "User Required")]
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        [Required(ErrorMessage = "Visitor-1 Phone Required")]
        public Nullable<decimal> phone { get; set; }
        public string SFirstName { get; set; }
        public string SLastName { get; set; }
        public string SCompany { get; set; }
        public Nullable<decimal> SPhone { get; set; }
        public string TFirstName { get; set; }
        public string TLastName { get; set; }
        public string TCompany { get; set; }
        public Nullable<decimal> TPhone { get; set; }
        public string FFirstName { get; set; }
        public string FLastName { get; set; }
        public string FCompany { get; set; }
        public Nullable<decimal> FPhone { get; set; }
        public Nullable<int> TicketID { get; set; }
        public string Prefix { get; set; }

        public List<DataCenterModel> ListDC { get; set; }
        public List<AccessModel> ListAccess { get; set; }
        public UserModel UserDetails { get; set; }
        public DataCenterModel DCDetails { get; set; }
        public List<CompanyModel> ListComp { get; set; }
        public List<UserModel> UserList { get; set; }
    }
}
