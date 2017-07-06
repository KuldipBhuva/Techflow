using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class UserModelAPI
    {
        public bool response { get; set; }
        public int UID { get; set; }
        public Nullable<int> CompID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
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
        public Nullable<int> Role { get; set; }
        
       
        public List<CompanyModel> ListComp { get; set; }
        public List<UserModelAPI> ListUser { get; set; }
        public CompanyModel CompDetails { get; set; }
    }
   
}
