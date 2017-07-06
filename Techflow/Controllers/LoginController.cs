using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.ViewModel;
using Services;

namespace Techflow.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index()
        {
            Session["UID"] = null;
            Session["CompID"] = null;
            Session["Name"] = null;
            Session["ReqQ"] = null;
            Session["Power"] = null;
            Session["Access"] = null;
            Session["Tickets"] = null;
            Session["Portal"] = null;
            Session["Invoice"] = null;
            Session["Role"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserModel model)
        {
            UserService objService = new UserService();
            UserModel objModel = new UserModel();

            objModel = objService.GetAuthUser(model.UserName, model.Password);
            if (objModel != null)
            {
                if (objModel.Portal == true && objModel.Status == 1)
                {
                    Session["UID"] = objModel.UID;
                    Session["Role"] = objModel.Role;
                    Session["CompID"] = objModel.CompID;
                    Session["Name"] = objModel.Title + ' ' + objModel.FirstName + ' ' + objModel.LastName;
                    Session["ReqQ"] = objModel.ReqQ;
                    Session["Power"] = objModel.Power;
                    Session["Access"] = objModel.Access;
                    Session["Tickets"] = objModel.Tickets;
                    Session["Portal"] = objModel.Portal;
                    Session["Invoice"] = objModel.Invoice;
                    Response.Redirect("/Dashboard/Index");
                    return RedirectToAction("/Dashboard/Index");
                }
                else
                {
                    TempData["EMsg"] = "You are not able to access portal.contact to admin.";
                    return View();
                }
            }
            else
            {
                TempData["EMsg"] = "Username or Password not Matched!!";
                return View();
            }

        }
        [HttpPost]
        public ActionResult ResetPwd(FormCollection All)
        {
      
           string email = All["txtEmail"];        

            var lst = Dbcontext.UserMasters.Where(m => m.Email == email).SingleOrDefault();
            int uid = 0;
            
            if (lst!=null)
            {
                uid = lst.UID;
                UserMaster um = Dbcontext.UserMasters.Where(m => m.UID == uid).SingleOrDefault();
                um.UserName = lst.Email;
                um.Password = lst.Phone;
                Dbcontext.SaveChanges();
                ////TempData["AMsg"] = "Password changed successfully.";

                TempData["UserMsg"] = "Password has been reset, you can login with email as username and phone as password";

                Response.Redirect("/Login/Index");
                return Content("<script language='javascript' type='text/javascript'>alert('Password has been reset, you can login with email as username and phone as password');</script>");
            }
            else
            {
                //TempData["AMsg"] = "Status Not Updated";
                //return Content("<script language='javascript' type='text/javascript'>alert('New Password and Confirm New Password Not Matched.');</script>");
                TempData["UserMsg"] = "Email Not Matched.";
                return RedirectToAction("Index","Login");
            }
        }

    }
}
