using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class InvoiceModel
    {
        public int IID { get; set; }
        [Required(ErrorMessage = "Company Required")]
        public Nullable<int> CompID { get; set; }
        [Required(ErrorMessage = "Amount Required")]
        public Nullable<decimal> Amount { get; set; }
        [Required(ErrorMessage = "Due Date Required")]
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<int> Status { get; set; }
        public string FileName { get; set; }
        public string FileURL { get; set; }

        public List<CompanyModel> ListComp { get; set; }
        public List<InvoiceModel> ListInvoice { get; set; }
        public CompanyModel CompDetails { get; set; }
    }
}
