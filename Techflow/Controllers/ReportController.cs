using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.ViewModel;
using Newtonsoft.Json;
using Services;

namespace Techflow.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            TicketModel objModel = new TicketModel();

            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Index(TicketModel model)
        {
            ReportService objService = new ReportService();
            TicketModel objModel = new TicketModel();
            List<TicketModel> lstTickets = new List<TicketModel>();
            DateTime todt = Convert.ToDateTime(model.ToDate);
            DateTime frmdt = Convert.ToDateTime(model.FromDate);
            int uid = Convert.ToInt32(model.CreatedBy);
            int cid = Convert.ToInt32(model.comp);

            lstTickets = objService.getTicketData(todt,frmdt, uid,cid);
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(lstTickets);

            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);
            return View(objModel);
        }
        public ActionResult TicketLog()
        {
            TicketModel objModel = new TicketModel();

            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);
            return View(objModel);
        }
        [HttpPost]
        public ActionResult TicketLog(TicketModel model)
        {
            ReportService objService = new ReportService();
            TicketModel objModel = new TicketModel();
            List<TicketModel> lstTickets = new List<TicketModel>();
            DateTime todt = Convert.ToDateTime(model.ToDate);
            DateTime frmdt = Convert.ToDateTime(model.FromDate);
            int uid = Convert.ToInt32(model.CreatedBy);
            int cid = Convert.ToInt32(model.comp);

            lstTickets = objService.getTicketData(todt, frmdt, uid, cid);
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(lstTickets);

            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            var lstOpen = lstTickets.Where(m => m.TicketStatusID == 1);
            var lstTicketO = from e in lstOpen
                                  select new
                                  {                                      
                                      label = e.CompDetails.Name,                                     
                                      y = lstOpen.Count(),
                                                            
                                  };
            var rowsO = lstTicketO;
            ViewBag.TicketOpen = JsonConvert.SerializeObject(rowsO);

            var lstResolved = lstTickets.Where(m => m.TicketStatusID == 2).ToList();
            var lstTicketR = from e in lstResolved
                            select new
                            {
                                label = e.CompDetails.Name,
                                y = lstResolved.Count(),

                            };
            var rowsR = lstTicketR.ToArray();
            ViewBag.TicketResolved = JsonConvert.SerializeObject(rowsR);

            return View(objModel);
        }
    }
}
