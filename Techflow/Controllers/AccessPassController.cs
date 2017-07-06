using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.ViewModel;
using Services;

namespace Techflow.Controllers
{
    public class AccessPassController : Controller
    {
        //
        // GET: /AccessPass/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index()
        {
            AccessPassService objService = new AccessPassService();
            AccessPassModel objModel = new AccessPassModel();
            List<AccessPassModel> objList = new List<AccessPassModel>();
             int cid=0;
            if (Session["CompID"] != null)
            {
                cid = Convert.ToInt32(Session["CompID"].ToString());
            }
            objList = objService.getAPData(cid);
            objModel.ListAP = new List<AccessPassModel>();
            objModel.ListAP.AddRange(objList);

            UserService objServiceComp = new UserService();
            List<CompanyModel> ListComp = new List<CompanyModel>();
            ListComp = objServiceComp.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(ListComp);

            return View(objModel);
        }
        [HttpPost]
        public ActionResult addFile(HttpPostedFileBase[] file)
        {
            if (file != null || Request.Files.Count > 0 || file[0].FileName != null || file[0].ContentLength != null || file[0].InputStream != null)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                string filename = Request.Files["file"].FileName;
                if (extension != "")
                {
                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/DataFiles"), Request.Files["file"].FileName);
                    if (System.IO.File.Exists(path1))
                        System.IO.File.Delete(path1);

                    Request.Files["file"].SaveAs(path1);
                    //string sqlConnectionString = @"Data Source=TESTSERVERVPD;Database=techflow;Trusted_Connection=true;Persist Security Info=True";
                    //string sqlConnectionString = @"data source=TESTSERVERVPD;initial catalog=techflow;user id=sa;password=newtech@123;MultipleActiveResultSets=True;App=EntityFramework";
                    //string sqlConnectionString = @"data source=208.91.198.59;initial catalog=techflow;user id=techflow;password=Newtech@009;MultipleActiveResultSets=True;App=EntityFramework";
                    string sqlConnectionString = @"data source=mydbinstance.c0cgp66jg3yv.ap-southeast-2.rds.amazonaws.com;initial catalog=techflow_online;user id=techflowdbun;password=TFpassw0rd16;MultipleActiveResultSets=True;App=EntityFramework";
                    //Create connection string to Excel work book
                    //string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                     string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';Persist Security Info=False";
                    //Create Connection to Excel work book
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    //Create OleDbCommand to fetch data from Excel

                    OleDbCommand cmd = new OleDbCommand("Select [APID],[CompID] ,[FirstName],[LastName],[AccessPassID],[SwipeDate],[SwipeTime],[DoorID] from [Sheet1$]", excelConnection);

                    excelConnection.Open();
                    OleDbDataReader dReader;
                    dReader = cmd.ExecuteReader();

                    SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlConnectionString);
                    //Give your Destination table name
                    sqlBulk.DestinationTableName = "AccessPassMaster";
                    sqlBulk.WriteToServer(dReader);
                    excelConnection.Close();
                    file = null;

                    // SQL Server Connection String



                    //objList = objService.getPDetail();
                    //objModel.ListPDetail = new List<PDetailModel>();
                    //objModel.ListPDetail.AddRange(objList);
                    //foreach (var a in objModel.ListPDetail)
                    //{
                    //    int? ptypeid = model.PTypeID;
                    //    int? schemeID = model.SchemeID;
                    //    int? status = 0;
                    //    objService.Update(ptypeid, schemeID, status);
                    //}
                    TempData["Msg"] = filename + " Uploaded Successfully.";
                }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Index(AccessPassModel model)
        {
            AccessPassService objService = new AccessPassService();
            AccessPassModel objModel = new AccessPassModel();
            List<AccessPassModel> objList = new List<AccessPassModel>();
            int cid = 0;
            if (Session["CompID"] != null)
            {
                cid = Convert.ToInt32(Session["CompID"].ToString());
            }
            int mm = Convert.ToInt32(model.month);
            int yy = Convert.ToInt32(model.year);
            objList = objService.getAPDataByMMYY(cid,mm,yy);
            objModel.ListAP = new List<AccessPassModel>();
            objModel.ListAP.AddRange(objList);

            UserService objServiceComp = new UserService();
            List<CompanyModel> ListComp = new List<CompanyModel>();
            ListComp = objServiceComp.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(ListComp);

            return View(objModel);
            
        }
        public ActionResult Edit(int id)
        {
            AccessPassService objService = new AccessPassService();
            AccessPassModel objModel = new AccessPassModel();

            objModel = objService.getByID(id);

            UserService objService1 = new UserService();
            List<CompanyModel> ListComp = new List<CompanyModel>();
            ListComp = objService1.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(ListComp);

            
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Edit(AccessPassModel model)
        {
            AccessPassService objService = new AccessPassService();
            model.SwipeTime = Convert.ToDateTime(model.SwipeTime);
            objService.Update(model);
            TempData["Msg"] = "Updated Successfully.";
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            try
            {                
                AccessPassService objService = new AccessPassService();
                AccessPassModel objModel = new AccessPassModel();
                objModel = objService.getByID(id);
                Dbcontext.AccessPassMasters.Remove(Dbcontext.AccessPassMasters.Find(id));
                TempData["Msg"] = "Record Deleted.";
                Dbcontext.SaveChanges();                
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Record Not Deleted.";                
                return View("Error");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DelData(AccessPassModel model)
        {
            try
            {
                int mm = model.month;
                int yy = model.year;
                int cid = 0;
                if (model.CompID != null && model.CompID != 0)
                {
                    cid = Convert.ToInt32(model.CompID);
                }
                AccessPassService objService = new AccessPassService();
                AccessPassModel objModel = new AccessPassModel();
                List<AccessPassModel> objList = new List<AccessPassModel>();

                objList = objService.getAPDataByMMYY(cid,mm,yy);
                objModel.ListAP = new List<AccessPassModel>();
                objModel.ListAP.AddRange(objList);
                foreach (var i in objModel.ListAP)
                {
                    int id = i.APID;
                    objModel = objService.getByID(id);
                    Dbcontext.AccessPassMasters.Remove(Dbcontext.AccessPassMasters.Find(id));
                    Dbcontext.SaveChanges();
                }
                int c = objList.Count();
                TempData["Msg"] = c + "Record Deleted.";

            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Record Not Deleted. " + ex;
                return View("Error");
            }
            return RedirectToAction("Index");
        }
    }
}
