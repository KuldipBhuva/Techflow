using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class UserModel
    {
        public bool reset { get; set; }
        public int UID { get; set; }
        [Required(ErrorMessage = "Company Required")]
        [Display(Name = "* Company")]
        public Nullable<int> CompID { get; set; }
        [Required(ErrorMessage = "Title Required")]
        [Display(Name = "* Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "First Name Required")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Username Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone Required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> ReqQ { get; set; }
        public Nullable<bool> Power { get; set; }
        public Nullable<bool> Access { get; set; }
        public Nullable<bool> Tickets { get; set; }
        public Nullable<bool> Portal { get; set; }
        public Nullable<bool> Invoice { get; set; }
        [Required(ErrorMessage = "Role Required")]
        public Nullable<int> Role { get; set; }
        [Required(ErrorMessage = "Mobile Required")]
        public Nullable<decimal> mobile { get; set; }


        public List<CompanyModel> ListComp { get; set; }
        public List<UserModel> ListUser { get; set; }
        public CompanyModel CompDetails { get; set; }
    }
}
