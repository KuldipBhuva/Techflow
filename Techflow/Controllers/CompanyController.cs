using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.ViewModel;
using Services;

namespace Techflow.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Company/

        public ActionResult Index()
        {
            CompanyService objService = new CompanyService();
            List<CompanyModel> objList = new List<CompanyModel>();
            CompanyModel objModel = new CompanyModel();

            objList = objService.getComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objList);
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Index(CompanyModel model)
        {
            CompanyService objService = new CompanyService();
            model.Status = 1;
            objService.InsertComp(model);
            TempData["Msg"] = "Successfully Added.";
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            CompanyService objService = new CompanyService();
            List<CompanyModel> objList = new List<CompanyModel>();
            CompanyModel objModel = new CompanyModel();

            objModel = objService.getByID(id);

            objList = objService.getComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objList);

            
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Edit(CompanyModel model)
        {
            CompanyService objService = new CompanyService();
            objService.Update(model);
            TempData["Msg"] = "Updated Successfully.";
            return RedirectToAction("Index");
        }
    }
}
