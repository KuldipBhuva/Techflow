using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class CompanyModel
    {
        public int CompID { get; set; }
        [Required(ErrorMessage = "Company Name Required")]
        [Display(Name = "* Company Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Legal Name Required")]
        [Display(Name = "* Legal Name")]
        public string LegalName { get; set; }
        public string ACType { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Phone Required")]
        [Display(Name = "* Phone")]
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string Fax { get; set; }
        [Required(ErrorMessage = "Email Required")]
        [Display(Name = "* Email")]
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Website { get; set; }
        public string BStreet { get; set; }
        public string BCity { get; set; }
        public string BState { get; set; }
        public string BPostCode { get; set; }
        public string BCountry { get; set; }
        public string SStreet { get; set; }
        public string SCity { get; set; }
        public string SState { get; set; }
        public string SPostCode { get; set; }
        public string SCountry { get; set; }
        public string ContactPerson { get; set; }
        public string ABN { get; set; }
        public string BHour { get; set; }
        public string ServiceStatus { get; set; }
        public string rackcombination { get; set; }
        public Nullable<int> Status { get; set; }

        public List<CompanyModel> ListComp { get; set; }
    }
}
