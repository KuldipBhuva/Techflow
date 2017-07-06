using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.ViewModel;
using Newtonsoft.Json;
using Services;

namespace Techflow.Controllers
{
    public class PowerUsageController : Controller
    {
        //
        // GET: /PowerUsage/
        
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult addFile(HttpPostedFileBase[] file,FormCollection All)
        {

            PowerUsageService objService = new PowerUsageService();
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
                    string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';Persist Security Info=False";
                    //string excelConnectionString = @"Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};DBQ=" + path1 + ";Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties='Excel 8.0;HDR=YES;IMEX=1;';Persist Security Info=False";
                    //Create Connection to Excel work book
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    //Create OleDbCommand to fetch data from Excel

                    OleDbCommand cmd = new OleDbCommand("Select [PUID],[CompID] ,[DataSuite],[RackID],[PowerFeeds],[AmpereRating],[PowerContracted(kW)],[PowerUsed(kW)],[Over/Under(kW)],[LastReading] from [Sheet1$]", excelConnection);

                    excelConnection.Open();
                    OleDbDataReader dReader;
                    dReader = cmd.ExecuteReader();

                    SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlConnectionString);
                    //Give your Destination table name
                    sqlBulk.DestinationTableName = "PowerUsageMaster";
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
        public ActionResult Index()
        {
            PowerUsageService objService = new PowerUsageService();
            PowerUsageModel objModel = new PowerUsageModel();
            List<PowerUsageModel> objList = new List<PowerUsageModel>();
            int cid=0;
            if (Session["CompID"] != null)
            {
                cid = Convert.ToInt32(Session["CompID"].ToString());
            }
            objList = objService.getPUData(cid);
            objModel.ListPowerData = new List<PowerUsageModel>();
            objModel.ListPowerData.AddRange(objList);


            List<PowerUsageModel> lstAppData = new List<PowerUsageModel>();
            PowerUsageModel objAppoint = new PowerUsageModel();
            PowerUsageService objAppService = new PowerUsageService();

         
            lstAppData = objAppService.getPUData(cid);

            var powerDataList = from e in lstAppData
                                select new
                                {
                                    //id = e.PUID,
                                    label = e.DataSuite+"("+e.RackID+")",
                                    //rackid=e.RackID,
                                    y = e.PowerContracted,
                                    //pu=e.PowerUsed,
                                    //ou=e.OverUnder,
                                    lastreading = e.LastReading.Value.ToString("MMMM-yyyy"),                                
                                };
            var rows = powerDataList.ToArray();
            if (rows.Count() > 0)
            {
                ViewBag.last = rows[0].lastreading;
            }
            string pc = JsonConvert.SerializeObject(rows);
            string pcf = pc.Replace("[", string.Empty).Replace("]", string.Empty);
            ViewBag.DataPointsPC = pcf;

            var powerDataListPU = from e in lstAppData
                                select new
                                {
                                    //id = e.PUID,
                                    label = e.DataSuite + "(" + e.RackID + ")",
                                    //rackid=e.RackID,
                                    y = e.PowerUsed,
                                    //pu=e.PowerUsed,
                                    //ou=e.OverUnder,
                                    //lastreading = e.LastReading.Value.ToString("yyyy-MM-dd") + " " + e.LastReading,                                
                                };
            var rowsPU = powerDataListPU.ToArray();
            string pu = JsonConvert.SerializeObject(rowsPU); ;
            string puf = pu.Replace("[", string.Empty).Replace("]", string.Empty);
            ViewBag.DataPointsPU = puf;

            List<PowerUsageModel> lstTotalPU = new List<PowerUsageModel>();
            //string year = Convert.ToString(System.DateTime.Now.Year);
            string year = "";
            if (objList.Count > 0)
            {
                year = objList[0].LastReading.Value.ToString("yyyy");
            }
            lstTotalPU = objService.getTotalPU(year,cid);
            objModel.ListTotalTickets = new List<PowerUsageModel>();
            objModel.ListTotalTickets.AddRange(lstTotalPU);

            var TTPCList = from e in lstTotalPU
                                select new
                                {
                                    //id = e.PUID,
                                    label = e.TTmonth,
                                    //rackid=e.RackID,
                                    y = e.TotalPC,
                                    //pu=e.PowerUsed,
                                    //ou=e.OverUnder,
                                    year = e.TTyear,
                                };
            var rowTT = TTPCList.ToArray();
            if (rowTT.Count() > 0)
            {
                ViewBag.Year = rowTT[0].year;
            }
            string ttpc = JsonConvert.SerializeObject(rowTT);
            string ttpcf = ttpc.Replace("[", string.Empty).Replace("]", string.Empty);
            ViewBag.TTPC = ttpcf;


            var TTPUList = from e in lstTotalPU
                                  select new
                                  {
                                      //id = e.PUID,
                                      label = e.TTmonth,
                                      //rackid=e.RackID,
                                      y = e.TotalPU,
                                      
                                  };
            var rowsTTPU = TTPUList.ToArray();
            string ttpu = JsonConvert.SerializeObject(rowsTTPU);
            string ttpuf = ttpu.Replace("[", string.Empty).Replace("]", string.Empty);
            ViewBag.TTPU = ttpuf;

            UserService objServiceComp = new UserService();
            List<CompanyModel> ListComp = new List<CompanyModel>();
            ListComp = objServiceComp.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(ListComp);

            return View(objModel);
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] file,PowerUsageModel model)
        {           
            PowerUsageModel objModel = new PowerUsageModel();
            PowerUsageService objService = new PowerUsageService();
            int mm = model.month;
            int yy = model.year;
            List<PowerUsageModel> objListSC = new List<PowerUsageModel>();
            int cid = 0;
            if (Session["CompID"] != null)
            {
                cid = Convert.ToInt32(Session["CompID"].ToString());
            }

            objListSC = objService.getPUDataByMMYY(mm, yy, cid);
            objModel.ListPowerData = new List<PowerUsageModel>();
            objModel.ListPowerData.AddRange(objListSC);
            System.Threading.Thread.Sleep(5000);


            List<PowerUsageModel> lstAppData = new List<PowerUsageModel>();
            PowerUsageModel objAppoint = new PowerUsageModel();
            PowerUsageService objAppService = new PowerUsageService();


            lstAppData = objAppService.getPUDataByMMYY(mm, yy, cid);

            var powerDataList = from e in lstAppData
                                select new
                                {
                                    //id = e.PUID,
                                    label = e.DataSuite + "(" + e.RackID + ")",
                                    //rackid=e.RackID,
                                    y = e.PowerContracted,
                                    //pu=e.PowerUsed,
                                    //ou=e.OverUnder,
                                    //lastreading = e.LastReading.Value.ToString("yyyy-MM-dd") + " " + e.LastReading,                                
                                };
            var rows = powerDataList.ToArray();
            string pc = JsonConvert.SerializeObject(rows);
            string pcf = pc.Replace("[", string.Empty).Replace("]", string.Empty);
            ViewBag.DataPointsPC = pcf;


            var powerDataListPU = from e in lstAppData
                                  select new
                                  {
                                      //id = e.PUID,
                                      label = e.DataSuite + "(" + e.RackID + ")",
                                      //rackid=e.RackID,
                                      y = e.PowerUsed,
                                      //pu=e.PowerUsed,
                                      //ou=e.OverUnder,
                                      lastreading = e.LastReading.Value.ToString("MMMM-yyyy"),
                                  };
            var rowsPU = powerDataListPU.ToArray();
            if (rowsPU.Count() > 0)
            {
                ViewBag.last = rowsPU[0].lastreading;
            }
            string pu = JsonConvert.SerializeObject(rowsPU); ;
            string puf = pu.Replace("[", string.Empty).Replace("]", string.Empty);
            ViewBag.DataPointsPU = puf;


            List<PowerUsageModel> lstTotalPU = new List<PowerUsageModel>();
            string year = Convert.ToString(model.year);
            lstTotalPU = objService.getTotalPU(year,cid);
            objModel.ListTotalTickets = new List<PowerUsageModel>();
            objModel.ListTotalTickets.AddRange(lstTotalPU);

            var TTPCList = from e in lstTotalPU
                           select new
                           {
                               //id = e.PUID,
                               label = e.TTmonth,
                               //rackid=e.RackID,
                               y = e.TotalPC,
                               //pu=e.PowerUsed,
                               //ou=e.OverUnder,
                               year = e.TTyear,
                           };
            var rowTT = TTPCList.ToArray();
            if (rowTT.Count() > 0)
            {
                ViewBag.Year = year;
            }
            string ttpc = JsonConvert.SerializeObject(rowTT);
            string ttpcf = ttpc.Replace("[", string.Empty).Replace("]", string.Empty);
            ViewBag.TTPC = ttpcf;


            var TTPUList = from e in lstTotalPU
                           select new
                           {
                               //id = e.PUID,
                               label = e.TTmonth,
                               //rackid=e.RackID,
                               y = e.TotalPU,

                           };
            var rowsTTPU = TTPUList.ToArray();
            string ttpu = JsonConvert.SerializeObject(rowsTTPU);
            string ttpuf = ttpu.Replace("[", string.Empty).Replace("]", string.Empty);
            ViewBag.TTPU = ttpuf;

            UserService objServiceComp = new UserService();
            List<CompanyModel> ListComp = new List<CompanyModel>();
            ListComp = objServiceComp.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(ListComp);
            //return Json(objModel.ListPowerData, JsonRequestBehavior.AllowGet);
            //var jsonResult = Json(objModel.ListPowerData, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
            //return PartialView("_Data", objModel);
            
            return View(objModel);
        } 
        public ActionResult Edit(int id)
        {
            PowerUsageService objService = new PowerUsageService();
            PowerUsageModel objModel = new PowerUsageModel();

            objModel = objService.getByID(id);

            UserService objService1 = new UserService();
            List<CompanyModel> ListComp = new List<CompanyModel>();
            ListComp = objService1.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(ListComp);
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Edit(PowerUsageModel model)
        {
            PowerUsageService objService = new PowerUsageService();
            objService.Update(model);
            TempData["Msg"] = "Updated Successfully.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DelData(PowerUsageModel model)
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
                PowerUsageModel objModel = new PowerUsageModel();
                PowerUsageService objService = new PowerUsageService();              
                List<PowerUsageModel> objListSC = new List<PowerUsageModel>();
                
                objListSC = objService.getPUDataByMMYY(mm, yy, cid);
                objModel.ListPowerData = new List<PowerUsageModel>();
                objModel.ListPowerData.AddRange(objListSC);
                foreach (var i in objModel.ListPowerData)
                {
                    int id = i.PUID;
                    objModel = objService.getByID(id);
                    Dbcontext.PowerUsageMasters.Remove(Dbcontext.PowerUsageMasters.Find(id));
                    Dbcontext.SaveChanges();
                }
                int c = objListSC.Count();
                TempData["Msg"] = c+"Record Deleted.";
                
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Record Not Deleted. "+ex;
                return View("Error");
            }
            return RedirectToAction("Index");
        }
    }
}
