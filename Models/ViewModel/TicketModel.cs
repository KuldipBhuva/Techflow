using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TicketModel
    {
        [Required(ErrorMessage = "From Date Required")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "To Date Required")]
        public DateTime? ToDate { get; set; }
        public Nullable<int> comp { get; set; }
        public int TicketID { get; set; }
        [Required(ErrorMessage = "Subject Required")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Description Required")]
        public string Description { get; set; }
        public Nullable<int> AssignTo { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string SenderEmail { get; set; }
        [Required(ErrorMessage = "Ticket Type Required")]
        public Nullable<int> TicketTypeID { get; set; }
        [Required(ErrorMessage = "Ticket Status Required")]
        public Nullable<int> TicketStatusID { get; set; }
        [Required(ErrorMessage = "Priority Required")]
        public Nullable<int> Priority { get; set; }
        public string Prefix { get; set; }


        #region TicketTran
        public int TTranID { get; set; }
        //public Nullable<int> TicketID { get; set; }
        //public Nullable<int> AssignTo { get; set; }
        //[Required(ErrorMessage = "Message Required")]
        public string Comment { get; set; }
        //public Nullable<int> TicketStatusID { get; set; }
        //public Nullable<int> CreatedBy { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //public Nullable<int> UpdatedBy { get; set; }
        //public Nullable<System.DateTime> UpdatedDate { get; set; }
        //public Nullable<bool> IsActive { get; set; }
        public Nullable<int> ReplayBy { get; set; }
        public Nullable<System.DateTime> ReplayDate { get; set; }
        #endregion

        public List<TicketModel> ListTicket { get; set; }
        public List<TicketTypeModel> ListTT { get; set; }
        public List<TicketStatusModel> ListTS { get; set; }

        public UserModel UserDetails { get; set; }
        public CompanyModel CompDetails { get; set; }
        public TicketStatusModel TStatusDetails { get; set; }
        public List<TicketStatusModel> ListTstatus { get; set; }
        public TicketTypeModel TTypeDetails { get; set; }
        public List<TicketTranModel> ListTicketTran { get; set; }
        public List<TicketAttachmentModel> ListTA { get; set; }
        public List<CompanyModel> ListComp { get; set; }
        public List<UserModel> UserList { get; set; }
        public QuotationModel Quotedetails { get; set; }
        public DeliveryModel Deldetails { get; set; }
        public AccessModel Accessdetails { get; set; }
    }
}
