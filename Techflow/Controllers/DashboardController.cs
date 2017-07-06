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
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index()
        {
            DashboardService objService = new DashboardService();
            TicketModel objModel = new TicketModel();
            List<TicketModel> objTList = new List<TicketModel>();

            objTList = objService.getAllTicket();
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(objTList);

            TicketService objServiceTS = new TicketService();
            List<TicketStatusModel> objTStatus = new List<TicketStatusModel>();
            objTStatus = objServiceTS.getTicketStatus();
            objModel.ListTstatus = new List<TicketStatusModel>();
            objModel.ListTstatus.AddRange(objTStatus);

            return View(objModel);
        }
        [HttpPost]
        public ActionResult Index(TicketModel model)
        {
            try
            {
                //int status = Convert.ToInt32(All["ddlStatus"]);
                //int tid = Convert.ToInt32(All["TicketID"]);
                int uid = 0;
                if (Session["UID"] != null)
                {
                    uid = Convert.ToInt32(Session["UID"].ToString());
                }
                TicketMaster tm = Dbcontext.TicketMasters.Where(m => m.TicketID == model.TicketID).SingleOrDefault();
                tm.TicketStatusID = model.TicketStatusID;
                tm.UpdatedDate = System.DateTime.Now;
                tm.UpdatedBy = uid;
                Dbcontext.SaveChanges();
                TempData["AMsg"] = "Status updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["AMsg"] = "Status Not Updated";
            }
            //return Content("<script language='javascript' type='text/javascript'>alert('Status updated successfully.');</script>");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult ChangePwd(FormCollection All)
        {
            int uid = 0;
            string opwd = All["txtOpwd"];
            string email = All["txtEmail"];
            string npwd = All["txtNpwd"];
            string ncpwd = All["txtNCpwd"];
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
            }

            var lst = Dbcontext.UserMasters.Where(m => m.UID == uid).SingleOrDefault();

            if (lst.Password == opwd && lst.Email == email)
            {

                if (npwd == ncpwd)
                {
                    UserMaster um = Dbcontext.UserMasters.Where(m => m.UID == uid).SingleOrDefault();
                    um.Password = ncpwd;
                    Dbcontext.SaveChanges();
                    ////TempData["AMsg"] = "Password changed successfully.";

                    TempData["UserMsg"] = "Password changed successfully.";
                    
                    Response.Redirect("/Login/Index");
                    return Content("<script language='javascript' type='text/javascript'>alert('Password changed successfully.');</script>");
                }
                else
                {
                    //TempData["AMsg"] = "Status Not Updated";
                    //return Content("<script language='javascript' type='text/javascript'>alert('New Password and Confirm New Password Not Matched.');</script>");
                    TempData["AMsg"] = "New Password and Confirm New Password Not Matched.";
                    return RedirectToAction("Index");
                }
            }
            else if (lst.Password != opwd)
            {
                //return Content("<script language='javascript' type='text/javascript'>alert('Old Password Not Matched.');</script>");
                TempData["AMsg"] = "Old Password Not Matched.";
                return RedirectToAction("Index");
            }
            else if (lst.Email != email)
            {
                //return Content("<script language='javascript' type='text/javascript'>alert('Email Not Matched.');</script>");
                TempData["AMsg"] = "Email Not Matched.";
                return RedirectToAction("Index");
            }
            else
            {
                //return Content("<script language='javascript' type='text/javascript'>alert('Error occured, Try again!!!');</script>");
                TempData["AMsg"] = "Error occured, Try again!!!";
                return RedirectToAction("Index");
            }

        }
    }
}
