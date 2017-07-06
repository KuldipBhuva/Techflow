using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TicketModelAPI
    {
        public bool response { get; set; }
        public int comp { get; set; }
        public int TicketID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Nullable<int> AssignTo { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string SenderEmail { get; set; }
        public Nullable<int> TicketTypeID { get; set; }
        public Nullable<int> TicketStatusID { get; set; }
        public Nullable<int> Priority { get; set; }


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
        public List<TicketModel> Listticket { get; set; }
    }

  
}
