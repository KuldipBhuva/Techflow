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
    public class UserController : Controller
    {
        //
        // GET: /User/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index()
        {
            UserService objService = new UserService();
            UserModel objModel = new UserModel();
            List<UserModel> objList = new List<UserModel>();
            int cid = 0;
            int rid = 0;
            if (Session["CompID"] != null)
            {
                cid = Convert.ToInt32(Session["CompID"].ToString());
                rid = Convert.ToInt32(Session["Role"].ToString());
            }
            objList = objService.getUser(cid, rid);
            objModel.ListUser = new List<UserModel>();
            objModel.ListUser.AddRange(objList);

            List<CompanyModel> ListComp = new List<CompanyModel>();
            ListComp = objService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(ListComp);

            return View(objModel);
        }
        [HttpPost]
        public ActionResult Index(UserModel model)
        {
            UserService objService = new UserService();
            model.Status = 1;
            model.UserName = model.Email;
            model.Password = model.Phone;
            if (model.Role == null || model.Role == 0)
            {
                model.Role = 3;
            }
            int cid = 0;
            if (Session["CompID"] != null)
            {
                cid = Convert.ToInt32(Session["CompID"].ToString());
            }
            if (model.CompID == null)
            {
                model.CompID = cid;
            }
            objService.InsertUser(model);
            TempData["Msg"] = "Successfully Added";
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            UserService objService = new UserService();
            UserModel objModel = new UserModel();
            List<UserModel> objList = new List<UserModel>();

            objModel = objService.getByID(id);
            int cid = 0;
            int rid = 0;
            if (Session["CompID"] != null)
            {
                cid = Convert.ToInt32(Session["CompID"].ToString());
                rid = Convert.ToInt32(Session["Role"].ToString());
            }
            objList = objService.getUser(cid, rid);
            objModel.ListUser = new List<UserModel>();
            objModel.ListUser.AddRange(objList);

            List<CompanyModel> ListComp = new List<CompanyModel>();
            ListComp = objService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(ListComp);
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            UserService objService = new UserService();
            objService.Update(model);

            if (model.reset == true)
            {
                UserMaster um = Dbcontext.UserMasters.Where(m => m.UID == model.UID).SingleOrDefault();
                um.Password = model.Phone;
                um.UserName = model.Email;
                Dbcontext.SaveChanges();
                TempData["AMsg"] = "Password reset successfully.";
            }
            TempData["Msg"] = "Updated Successfully.";

            return RedirectToAction("Index");

        }

        //public ActionResult ResetPwd(FormCollection All,UserModel model)
        //{
        //    int uid = 0;
        //    if (Session["UID"] != null)
        //    {
        //        uid = Convert.ToInt32(Session["UID"].ToString());
        //    }
        //    UserMaster um = Dbcontext.UserMasters.Where(m => m.UID == uid).SingleOrDefault();
        //    um.Password = model.Phone;
        //    um.UserName = model.Email;
        //    Dbcontext.SaveChanges();
        //    TempData["AMsg"] = "Password reset successfully.";
        //    return Content("<script language='javascript' type='text/javascript'>alert('Password changed successfully.');</script>");

        //}
    }
}
