using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.ViewModel;
using Services;

namespace Techflow.Controllers
{
    public class InvoiceController : Controller
    {
        //
        // GET: /Invoice/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index()
        {
            InvoiceModel objModel = new InvoiceModel();
            InvoiceService objService = new InvoiceService();
            List<InvoiceModel> ListInvoice = new List<InvoiceModel>();
            int cid = 0;
            int rid = 0;
            if (Session["CompID"] != null)
            {
                cid = Convert.ToInt32(Session["CompID"].ToString());
                rid = Convert.ToInt32(Session["Role"].ToString());

                ListInvoice = objService.getInvoice(cid, rid);
                objModel.ListInvoice = new List<InvoiceModel>();
                objModel.ListInvoice.AddRange(ListInvoice);

                QuotationService objCompUSerService = new QuotationService();
                List<CompanyModel> objCompList = new List<CompanyModel>();
                objCompList = objCompUSerService.getActiveComp();
                objModel.ListComp = new List<CompanyModel>();
                objModel.ListComp.AddRange(objCompList);
            }
            else
            {
                Response.Redirect("/Login/Index");
            }

            return View(objModel);
        }
        [HttpPost]
        public ActionResult Index(InvoiceModel model, HttpPostedFileBase[] files)
        {
            InvoiceService objService = new InvoiceService();
            if (files != null)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        string filename = System.IO.Path.GetFileName(file.FileName);
                        string folderPath = Server.MapPath("~/DataFiles/Invoice/");
                        //  obj.EmpId = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"].ToString());
                        string destinationPath = folderPath;
                        string employeeFolderPath = destinationPath;
                        // create folder if it is not exist
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                            if (!Directory.Exists(employeeFolderPath))
                            {
                                Directory.CreateDirectory(employeeFolderPath);
                                // create emp id folder of not exist
                            }
                        }
                        else
                        {
                            if (!Directory.Exists(employeeFolderPath))
                            {
                                Directory.CreateDirectory(employeeFolderPath);
                                // create emp id folder of not exist
                            }
                        }
                        destinationPath = employeeFolderPath;
                        /*Saving the file in server folder*/
                        //file.SaveAs(Server.MapPath("~/Images/" + filename));
                        string fileNewName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                        file.SaveAs(Path.Combine(destinationPath, fileNewName));

                        //objModel.CID = Convert.ToInt32(s);
                        model.FileURL = Path.Combine("/DataFiles/Invoice/", fileNewName);
                        model.FileName = Path.GetFileNameWithoutExtension(file.FileName);
                        model.Status = 1;//UnPaid,2=Paid,3=OverDue
                        objService.Insert(model);
                        TempData["Msg"] = "Invoice Added successfully.";
                    }
                    else
                    {
                        TempData["AMsg"] = "Attach invoice file in attachment.";
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            InvoiceService objService = new InvoiceService();
            InvoiceModel objModel = new InvoiceModel();

            objModel = objService.getByID(id);

            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Edit(InvoiceModel model, HttpPostedFileBase[] files)
        {
            InvoiceService objService = new InvoiceService();
            
            if (files != null)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        string filename = System.IO.Path.GetFileName(file.FileName);
                        string folderPath = Server.MapPath("~/DataFiles/Invoice/");
                        //  obj.EmpId = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"].ToString());
                        string destinationPath = folderPath;
                        string employeeFolderPath = destinationPath;
                        // create folder if it is not exist
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                            if (!Directory.Exists(employeeFolderPath))
                            {
                                Directory.CreateDirectory(employeeFolderPath);
                                // create emp id folder of not exist
                            }
                        }
                        else
                        {
                            if (!Directory.Exists(employeeFolderPath))
                            {
                                Directory.CreateDirectory(employeeFolderPath);
                                // create emp id folder of not exist
                            }
                        }
                        destinationPath = employeeFolderPath;
                        /*Saving the file in server folder*/
                        //file.SaveAs(Server.MapPath("~/Images/" + filename));
                        string fileNewName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                        file.SaveAs(Path.Combine(destinationPath, fileNewName));

                        //objModel.CID = Convert.ToInt32(s);
                        model.FileURL = Path.Combine("/DataFiles/Invoice/", fileNewName);
                        model.FileName = Path.GetFileNameWithoutExtension(file.FileName);                        
                        objService.Update(model);
                        TempData["Msg"] = "Updated Successfully.";
                    }
                    else
                    {
                        objService.Update(model);
                        TempData["Msg"] = "Updated Successfully.";
                    }
                }
            }
            
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            try
            {
                InvoiceService objService = new InvoiceService();
                InvoiceModel objModel = new InvoiceModel();
                objModel = objService.getByID(id);
                Dbcontext.InvoiceMasters.Remove(Dbcontext.InvoiceMasters.Find(id));
                TempData["Msg"] = "Invoice Deleted.";
                Dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Invoice Not Deleted.";
                return View("Error");
            }
            return RedirectToAction("Index");
        }
    }
}
