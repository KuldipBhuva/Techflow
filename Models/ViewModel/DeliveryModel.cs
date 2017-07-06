using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class DeliveryModel
    {
        [Required(ErrorMessage = "Company Required")]
        public int? comp { get; set; }
        public int BDID { get; set; }
        [Required(ErrorMessage = "Datacenter Required")]
        public Nullable<int> DataCenter { get; set; }
        [Required(ErrorMessage = "Firstname Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Company Required")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Start Date/Time Required")]
        public Nullable<System.DateTime> StDateTime { get; set; }
        [Required(ErrorMessage = "End Date/Time Required")]
        public Nullable<System.DateTime> EndDateTime { get; set; }
        [Required(ErrorMessage = "Boxes Required")]
        public Nullable<decimal> Boxes { get; set; }
        [Required(ErrorMessage = "Tracking No. Required")]
        public string TrackingNo { get; set; }
        [Required(ErrorMessage = "Project Ref. Required")]
        public string ProjectRef { get; set; }
        [Required(ErrorMessage = "Storage Required")]
        public Nullable<bool> Storage { get; set; }
        [Required(ErrorMessage = "Remote Hands Required")]
        public Nullable<bool> RemoteHands { get; set; }
        public Nullable<bool> IsActive { get; set; }
        [Required(ErrorMessage = "User Required")]
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> TicketID { get; set; }
        public string Prefix { get; set; }

        public List<DataCenterModel> ListDC { get; set; }
        public List<DeliveryModel> ListDelivery { get; set; }
        public UserModel UserDetails { get; set; }
        public DataCenterModel DCDetails { get; set; }
        public List<CompanyModel> ListComp { get; set; }
        public List<UserModel> UserList { get; set; }
    }
}
