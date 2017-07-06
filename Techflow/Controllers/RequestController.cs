using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.ViewModel;
using Services;
using System.Globalization;

namespace Techflow.Controllers
{
    public class RequestController : Controller
    {
        //
        // GET: /Request/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Quotation()
        {
            QuotationService objService = new QuotationService();
            QuotationModel objModel = new QuotationModel();
            List<QuotationModel> ListQ = new List<QuotationModel>();
            int uid = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
            }
            ListQ = objService.getQuotation(uid);
            objModel.ListQuo = new List<QuotationModel>();
            objModel.ListQuo.AddRange(ListQ);

            List<DataCenterModel> ListDC = new List<DataCenterModel>();
            ListDC = objService.getDataCenter();
            objModel.ListDC = new List<DataCenterModel>();
            objModel.ListDC.AddRange(ListDC);

            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            return View(objModel);
        }
        [HttpPost]
        public ActionResult Quotation(QuotationModel model, TicketModel model1, TicketTranModel model2)
        {
            QuotationService objService = new QuotationService();
            QuotationModel objModel = new QuotationModel();
            TicketModel TCobjModel = new TicketModel();
            TicketService TicobjService = new TicketService();
            int uid = 0;
            int? dcid = 0;
            if (Session["UID"] != null)
            {
                model.Prefix = "REQ";
                model1.Prefix = "REQ";
                model2.Prefix = "REQ";
                uid = Convert.ToInt32(Session["UID"].ToString());
                if (model.DataCenterOther != null)
                {
                    dcid = model.DataCenterOther;
                }
                else
                {
                    dcid = model.DataCenter;
                }
                model.DataCenter = dcid;
                model.IsActive = true;
                model.CreatedDate = System.DateTime.Now;
                if (model.CreatedBy == null)
                {
                    model.CreatedBy = uid;
                }
                if (model.ServiceReq == null && model.CableSource != null)
                {
                    model1.Subject = "Cable Quotation";
                    model1.Description = model.CableType;
                    model2.Comment = model.CableType;
                }
                else
                {
                    model1.Subject = "Other Quotation";
                    model1.Description = model.ServiceReq;
                    model2.Comment = model.ServiceReq;
                }
                if (model.CreatedBy == null)
                {
                    model1.CreatedBy = uid;
                }
                else
                {
                    model1.CreatedBy = model.CreatedBy;
                }
                model1.CreatedDate = System.DateTime.Now;
                model1.IsActive = true;
                model1.TicketTypeID = 1;
                model1.TicketStatusID = 1;
                model1.Priority = 4;

                TCobjModel = TicobjService.GetLastID();
                var dateTimeNow = DateTime.Now;
                var dateOnlyString = dateTimeNow.ToShortDateString();
                //DateTime dt = DateTime.ParseExact(objModel.CreatedDate.ToString(), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                //string createddt = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                DateTime dt = DateTime.Parse(objModel.CreatedDate.ToString());
                string createddt = dt.ToString("dd-MMM-yy", CultureInfo.InvariantCulture);
                if (createddt == dateOnlyString)
                {

                    model1.TicketID = TCobjModel.TicketID + 1;

                }
                else
                {
                    model1.TicketID = TCobjModel.TicketID + 38;

                }

                int tid = objService.Insert(model, model1);
                TicketService objService1 = new TicketService();
                model2.TicketID = tid;

                if (model.CreatedBy == null)
                {
                    model2.CreatedBy = uid;
                }
                else
                {
                    model2.CreatedBy = model.CreatedBy;
                }
                model2.CreatedDate = System.DateTime.Now;
                objService1.InsertTicketTran(model2);

                try
                {
                    var user = Dbcontext.UserMasters.Where(m => m.UID == model.CreatedBy).SingleOrDefault();
                    var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == user.CompID).SingleOrDefault();
                    //var fromAddress = new MailAddress("newtech_infosoft@yahoo.com", comp.Name);
                    //var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                    var fromAddress = new MailAddress("support@techflow.com.au", comp.Name);
                    var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                    //var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                    const string fromPassword = "sUpport##11";
                    var dc = Dbcontext.DataCenterMasters.Where(m => m.DCID == dcid).SingleOrDefault();

                    string subject = "Quotation Request " + model.Prefix + "" + tid;
                    string ReqDelDate = "";
                    string dateQ = "";
                    string dateService = "";
                    if (model.ReqDelDate != null) { ReqDelDate = model.ReqDelDate.Value.ToString("dd/MM/yyyy"); }
                    if (model.DateForQuote != null) { dateQ = model.DateForQuote.Value.ToString("dd/MM/yyyy"); }
                    if (model.DateForService != null) { dateService = model.DateForService.Value.ToString("dd/MM/yyyy"); }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html>");
                    sb.Append("<Body>");
                    sb.Append("<table><tr><td><b>Dear Techflow Administrator,</b></td></tr><tr><td><br/><b>Created By : </b></td><td>" + user.Title + ' ' + user.FirstName + ' ' + user.LastName + "</td></tr><tr><td><b>Source of Cable : </b></td><td>" + model.CableSource + "</td></tr><tr><td><b>Destination of Cable : </b></td><td>" + model.CableDest + "</td></tr><tr><td><b>Type of Cable : </b></td><td>" + model.CableType + "</td></tr><tr><td><b>Cable Termination at Source : </b></td><td>" + model.TerminationSource + "</td></tr><tr><td><b>Cable Termination at Destination : </b></td><td>" + model.TerminationDest + "</td></tr><tr><td><b>Rquested Delivery Date : </b></td><td>" + ReqDelDate + "</td></tr><tr><td><b>Premium Service Required : </b></td><td>" + model.IsPremium + "</td></tr><tr><td><b>Patch Panel : </b></td><td>" + model.PatchPanel + "</td></tr><tr><td><b>Media Converter : </b></td><td>" + model.MediaConverter + "</td></tr><tr><td><b>Additional Comments : </b></td><td>" + model.Comments + "</td></tr><tr><td><b>Details of Service : </b></td><td>" + model.ServiceReq + "</td></tr><tr><td><b>Requested Date for Quote : </b></td><td>" + dateQ + "</td></tr><tr><td><b>Requested Date for Service : </b></td><td>" + dateService + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + user.FirstName + ' ' + user.LastName + "</td></tr></table>");
                    sb.Append("</Body>");
                    sb.Append("</html>");

                    string body = sb.ToString();

                    var smtp = new SmtpClient
                    {
                        //Host = "smtp.mail.yahoo.com",
                        //Port = 25,
                        //EnableSsl = true,
                        //DeliveryMethod = SmtpDeliveryMethod.Network,
                        //UseDefaultCredentials = false,
                        //Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {

                        Subject = subject,
                        Body = body
                    })
                    {
                        message.IsBodyHtml = true;
                        //message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Distributer Order"));
                        //message.CC.Add(new MailAddress(Convert.ToString("dhrumilshah@newtechinfosoft.in"), "Dhrumil"));
                        //message.CC.Add(new MailAddress(Convert.ToString("info@sugamhealthcare.com"), "Info-Sugam Health Care"));
                        smtp.Send(message);
                        TempData["Msg"] = "Quotation Successfully Sent.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Sorry!!Quotation not sent try after some time.";
                }


            }
            return RedirectToAction("Quotation");
        }
        public ActionResult QView(int id)
        {
            QuotationService objService = new QuotationService();
            QuotationModel objModel = new QuotationModel();

            objModel = objService.getByID(id);


            List<DataCenterModel> ListDC = new List<DataCenterModel>();
            ListDC = objService.getDataCenter();
            objModel.ListDC = new List<DataCenterModel>();
            objModel.ListDC.AddRange(ListDC);

            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);


            var comp = Dbcontext.UserMasters.Where(m => m.UID == objModel.CreatedBy).SingleOrDefault();
            int? cid = comp.CompID;
            objModel.comp = cid;

            return View(objModel);
        }
        public ActionResult QDelete(int id)
        {
            try
            {
                QuotationService objService = new QuotationService();
                QuotationModel objModel = new QuotationModel();
                objModel = objService.getByID(id);
                Dbcontext.QuotationMasters.Remove(Dbcontext.QuotationMasters.Find(id));
                TempData["Msg"] = "Record Deleted.";
                Dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Record Not Deleted.";
                return View("Error");
            }
            return RedirectToAction("Quotation");
        }
        public ActionResult QEdit(int id)
        {
            QuotationService objService = new QuotationService();
            QuotationModel objModel = new QuotationModel();

            objModel = objService.getByID(id);

            List<DataCenterModel> ListDC = new List<DataCenterModel>();
            ListDC = objService.getDataCenter();
            objModel.ListDC = new List<DataCenterModel>();
            objModel.ListDC.AddRange(ListDC);

            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);
            var comp = Dbcontext.UserMasters.Where(m => m.UID == objModel.CreatedBy).SingleOrDefault();
            int? cid = comp.CompID;
            objModel.comp = cid;

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            return View(objModel);
        }
        [HttpPost]
        public ActionResult QEdit(QuotationModel model)
        {
            QuotationService objService = new QuotationService();
            int uid = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());


                model.UpdatedBy = uid;
                model.UpdatedDate = System.DateTime.Now;
                objService.Update(model);
                TempData["Msg"] = "Updated successfully.";
            }
            return RedirectToAction("Quotation");
        }
        public ActionResult Access()
        {
            AccessModel objModel = new AccessModel();
            AccessService objService = new AccessService();
            List<AccessModel> ListAccess = new List<AccessModel>();
            int uid = 0;
            int cid = 0;
            int role = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
            }
            if (Session["CompID"] != null)
            {
                cid = Convert.ToInt32(Session["CompID"].ToString());
                role = Convert.ToInt32(Session["Role"].ToString());
            }
            ListAccess = objService.getAccessReq(cid);
            objModel.ListAccess = new List<AccessModel>();
            objModel.ListAccess.AddRange(ListAccess);

            QuotationService objService1 = new QuotationService();
            List<DataCenterModel> ListDC = new List<DataCenterModel>();
            ListDC = objService1.getDataCenter();
            objModel.ListDC = new List<DataCenterModel>();
            objModel.ListDC.AddRange(ListDC);


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
        public ActionResult Access(AccessModel model, TicketModel model1, TicketTranModel model2)
        {
            AccessService objService = new AccessService();
            TicketModel objModel = new TicketModel();
            TicketService TicobjService = new TicketService();
            int uid = 0;
            if (Session["UID"] != null)
            {
                model.Prefix = "REQ";
                model1.Prefix = "REQ";
                model2.Prefix = "REQ";
                uid = Convert.ToInt32(Session["UID"].ToString());

                model.IsActive = true;
                model.CreatedDate = System.DateTime.Now;
                if (model.CreatedBy == null)
                {
                    model.CreatedBy = uid;
                    model1.CreatedBy = uid;
                }
                else
                {
                    model1.CreatedBy = model.CreatedBy;
                }
                model1.Subject = "Access Request";
                model1.Description = model.Reason;
                model1.TicketStatusID = 1;
                model1.TicketTypeID = 1;
                model1.Priority = 4;
                model1.IsActive = true;
                model1.CreatedDate = System.DateTime.Now;
                objModel = TicobjService.GetLastID();
                var dateTimeNow = DateTime.Now;
                var dateOnlyString = dateTimeNow.ToShortDateString();
                //DateTime dt = DateTime.ParseExact(objModel.CreatedDate.ToString(), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                //string createddt = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                DateTime dt = DateTime.Parse(objModel.CreatedDate.ToString());
                string createddt = dt.ToString("dd-MMM-yy", CultureInfo.InvariantCulture);
                if (createddt == dateOnlyString)
                {

                    model1.TicketID = objModel.TicketID + 1;

                }
                else
                {
                    model1.TicketID = objModel.TicketID + 38;

                }
                int tid = objService.Insert(model, model1);

                TicketService objService1 = new TicketService();
                model2.TicketID = tid;
                model2.Comment = model.Reason;

                if (model.CreatedBy == null)
                {
                    model2.CreatedBy = uid;
                }
                else
                {
                    model2.CreatedBy = model.CreatedBy;
                }
                model2.CreatedDate = System.DateTime.Now;
                objService1.InsertTicketTran(model2);

                try
                {
                    var user = Dbcontext.UserMasters.Where(m => m.UID == model.CreatedBy).SingleOrDefault();
                    var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == user.CompID).SingleOrDefault();
                    var fromAddress = new MailAddress("support@techflow.com.au", comp.Name);
                    var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                    //var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                    const string fromPassword = "sUpport##11";
                    var dc = Dbcontext.DataCenterMasters.Where(m => m.DCID == model.DataCenter).SingleOrDefault();

                    string subject = "Access Request " + " " + model.Prefix + "" + tid;
                    StringBuilder sb = new StringBuilder();     
                    sb.Append("<html>");
                    sb.Append("<Body>");
                    sb.Append("<table><tr><td><b>Dear Techflow Administrator,</b></td></tr><tr><td><br/><b>Created By : </b></td><td><br/>" + user.Title + ' ' + user.FirstName + ' ' + user.LastName + "</td></tr><tr><td><b>Data Centre : </b></td><td>" + dc.DataCenter + "</td></tr><tr><td><b>Name : </b></td><td>" + model.FirstName + ' ' + model.LastName + "</td></tr><tr><td><b>Company : </b></td><td>" + model.Company + "</td></tr><tr><td><b>Start Date/Time : </b></td><td>" + model.StDateTime + "</td></tr><tr><td><b>End Date/Time : </b></td><td>" + model.EndDateTime + "</td></tr><tr><td><b>Reason : </b></td><td>" + model.Reason + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + user.FirstName + ' ' + user.LastName + "</td></tr></table>");
                    sb.Append("</Body>");
                    sb.Append("</html>");

                    string body = sb.ToString();

                    var smtp = new SmtpClient
                    {

                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {

                        Subject = subject,
                        Body = body
                    })
                    {
                        message.IsBodyHtml = true;
                        smtp.Send(message);
                        TempData["Msg"] = "Your Access Request Successfully Sent.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Sorry!!Access Request not sent try after some time.";
                }
            }
            return RedirectToAction("Access");
        }
        public ActionResult AView(int id)
        {
            AccessService objService = new AccessService();
            AccessModel objModel = new AccessModel();

            objModel = objService.getByID(id);

            QuotationService objService1 = new QuotationService();
            List<DataCenterModel> ListDC = new List<DataCenterModel>();
            ListDC = objService1.getDataCenter();
            objModel.ListDC = new List<DataCenterModel>();
            objModel.ListDC.AddRange(ListDC);


            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            var comp = Dbcontext.UserMasters.Where(m => m.UID == objModel.CreatedBy).SingleOrDefault();
            int? cid = comp.CompID;
            objModel.comp = cid;

            return View(objModel);
        }
        public ActionResult ADelete(int id)
        {
            try
            {
                AccessService objService = new AccessService();
                AccessModel objModel = new AccessModel();
                objModel = objService.getByID(id);
                Dbcontext.AccessMasters.Remove(Dbcontext.AccessMasters.Find(id));
                TempData["Msg"] = "Record Deleted.";
                Dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Record Not Deleted.";
                return View("Error");
            }
            return RedirectToAction("Access");
        }
        public ActionResult AEdit(int id)
        {
            AccessService objService = new AccessService();
            AccessModel objModel = new AccessModel();

            objModel = objService.getByID(id);

            QuotationService objService1 = new QuotationService();
            List<DataCenterModel> ListDC = new List<DataCenterModel>();
            ListDC = objService1.getDataCenter();
            objModel.ListDC = new List<DataCenterModel>();
            objModel.ListDC.AddRange(ListDC);


            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            var comp = Dbcontext.UserMasters.Where(m => m.UID == objModel.CreatedBy).SingleOrDefault();
            int? cid = comp.CompID;
            objModel.comp = cid;

            return View(objModel);
        }
        [HttpPost]
        public ActionResult AEdit(AccessModel model)
        {
            AccessService objService = new AccessService();
            int uid = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());


                model.UpdatedBy = uid;
                model.UpdatedDate = System.DateTime.Now;
                objService.Update(model);
                TempData["Msg"] = "Updated successfully.";
            }
            return RedirectToAction("Access");
        }
        public ActionResult FillUser(int cid)
        {
            List<UserModel> lstModel = new List<UserModel>();
            UserModel objModel = new UserModel();
            QuotationService objService = new QuotationService();
            lstModel = objService.getUserByComp(cid);
            objModel.ListUser = new List<UserModel>();
            objModel.ListUser.AddRange(lstModel);
            System.Threading.Thread.Sleep(5000);
            return Json(objModel.ListUser, JsonRequestBehavior.AllowGet);
            //var jsonResult = Json(objModel.ListUser, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
        }
        public ActionResult Delivery()
        {
            DeliveryModel objModel = new DeliveryModel();
            DeliveryService objService = new DeliveryService();
            List<DeliveryModel> ListAccess = new List<DeliveryModel>();
            int uid = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
            }
            ListAccess = objService.getDeliveryReq(uid);
            objModel.ListDelivery = new List<DeliveryModel>();
            objModel.ListDelivery.AddRange(ListAccess);

            QuotationService objService1 = new QuotationService();
            List<DataCenterModel> ListDC = new List<DataCenterModel>();
            ListDC = objService1.getDataCenter();
            objModel.ListDC = new List<DataCenterModel>();
            objModel.ListDC.AddRange(ListDC);


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
        public ActionResult Delivery(DeliveryModel model, TicketModel model1, TicketTranModel model2)
        {
            TicketModel objModel = new TicketModel();
            TicketService TicobjService = new TicketService();
            DeliveryService objService = new DeliveryService();
            int uid = 0;
            if (Session["UID"] != null)
            {
                model.Prefix = "REQ";
                model1.Prefix = "REQ";
                model2.Prefix = "REQ";
                uid = Convert.ToInt32(Session["UID"].ToString());

                model.IsActive = true;
                model.CreatedDate = System.DateTime.Now;
                if (model.CreatedBy == null)
                {
                    model.CreatedBy = uid;
                    model1.CreatedBy = uid;
                }
                else
                {
                    model1.CreatedBy = model.CreatedBy;
                }
                model1.Subject = "Delivery Request";
                model1.Description = model.ProjectRef;
                model1.CreatedDate = System.DateTime.Now;
                model1.IsActive = true;
                model1.TicketTypeID = 1;
                model1.TicketStatusID = 1;
                model1.Priority = 4;
                objModel = TicobjService.GetLastID();
                var dateTimeNow = DateTime.Now;
                var dateOnlyString = dateTimeNow.ToShortDateString();
                //DateTime dt = DateTime.ParseExact(objModel.CreatedDate.ToString(), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                //string createddt = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                DateTime dt = DateTime.Parse(objModel.CreatedDate.ToString());
                string createddt = dt.ToString("dd-MMM-yy", CultureInfo.InvariantCulture);
                if (createddt == dateOnlyString)
                {

                    model1.TicketID = objModel.TicketID + 1;

                }
                else
                {
                    model1.TicketID = objModel.TicketID + 38;

                }
                int tid = objService.Insert(model, model1);

                TicketService objService1 = new TicketService();
                model2.TicketID = tid;
                model2.Comment = model.ProjectRef;

                if (model.CreatedBy == null)
                {
                    model2.CreatedBy = uid;
                }
                else
                {
                    model2.CreatedBy = model.CreatedBy;
                }
                model2.CreatedDate = System.DateTime.Now;
                objService1.InsertTicketTran(model2);

                try
                {
                    var user = Dbcontext.UserMasters.Where(m => m.UID == model.CreatedBy).SingleOrDefault();
                    var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == user.CompID).SingleOrDefault();
                    var fromAddress = new MailAddress("support@techflow.com.au", comp.Name);
                    var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                    //var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                    const string fromPassword = "sUpport##11";
                    var dc = Dbcontext.DataCenterMasters.Where(m => m.DCID == model.DataCenter).SingleOrDefault();

                    string subject = "Delivery Request " + model.Prefix + "" + tid;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html>");
                    sb.Append("<Body>");
                    sb.Append("<table><tr><td><b>Dear Techflow Administrator,</b></td></tr><tr><td><br/><b>Created By : </b></td><td><br/>" + user.Title + ' ' + user.FirstName + ' ' + user.LastName + "</td></tr><tr><td><b>Data Centre : </b></td><td>" + dc.DataCenter + "</td></tr><tr><td><b>Name : </b></td><td>" + model.FirstName + ' ' + model.LastName + "</td></tr><tr><td><b>Company : </b></td><td>" + model.Company + "</td></tr><tr><td><b>Start Date/Time : </b></td><td>" + model.StDateTime + "</td></tr><tr><td><b>End Date/Time : </b></td><td>" + model.EndDateTime + "</td></tr><tr><td><b>Boxes : </b></td><td>" + model.Boxes + "</td></tr><tr><td><b>Tracking No. : </b></td><td>" + model.TrackingNo + "</td></tr><tr><td><b>Project Reference : </b></td><td>" + model.ProjectRef + "</td></tr><tr><td><b>Storage : </b></td><td>" + model.Storage + "</td></tr><tr><td><b>Remote Hands : </b></td><td>" + model.RemoteHands + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + user.FirstName + ' ' + user.LastName + "</td></tr></table>");
                    sb.Append("</Body>");
                    sb.Append("</html>");

                    string body = sb.ToString();

                    var smtp = new SmtpClient
                    {
                        //Host = "smtp.mail.yahoo.com",
                        //Port = 25,
                        //EnableSsl = true,
                        //DeliveryMethod = SmtpDeliveryMethod.Network,
                        //UseDefaultCredentials = false,
                        //Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {

                        Subject = subject,
                        Body = body
                    })
                    {
                        message.IsBodyHtml = true;
                        //message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Distributer Order"));
                        //message.CC.Add(new MailAddress(Convert.ToString("dhrumilshah@newtechinfosoft.in"), "Dhrumil"));
                        //message.CC.Add(new MailAddress(Convert.ToString("info@sugamhealthcare.com"), "Info-Sugam Health Care"));
                        smtp.Send(message);
                        TempData["Msg"] = "Your Delivery Request Successfully Sent.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Sorry!!Delivery Request not sent try after some time.";
                }


            }
            return RedirectToAction("Delivery");
        }
        public ActionResult DView(int id)
        {
            DeliveryService objService = new DeliveryService();
            DeliveryModel objModel = new DeliveryModel();

            objModel = objService.getByID(id);

            QuotationService objService1 = new QuotationService();
            List<DataCenterModel> ListDC = new List<DataCenterModel>();
            ListDC = objService1.getDataCenter();
            objModel.ListDC = new List<DataCenterModel>();
            objModel.ListDC.AddRange(ListDC);


            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            var comp = Dbcontext.UserMasters.Where(m => m.UID == objModel.CreatedBy).SingleOrDefault();
            int? cid = comp.CompID;
            objModel.comp = cid;

            return View(objModel);
        }
        public ActionResult DDelete(int id)
        {
            try
            {
                DeliveryService objService = new DeliveryService();
                DeliveryModel objModel = new DeliveryModel();
                objModel = objService.getByID(id);
                Dbcontext.DeliveryMasters.Remove(Dbcontext.DeliveryMasters.Find(id));
                TempData["Msg"] = "Record Deleted.";
                Dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Record Not Deleted.";
                return View("Error");
            }
            return RedirectToAction("Delivery");
        }
        public ActionResult DEdit(int id)
        {
            DeliveryService objService = new DeliveryService();
            DeliveryModel objModel = new DeliveryModel();

            objModel = objService.getByID(id);

            QuotationService objService1 = new QuotationService();
            List<DataCenterModel> ListDC = new List<DataCenterModel>();
            ListDC = objService1.getDataCenter();
            objModel.ListDC = new List<DataCenterModel>();
            objModel.ListDC.AddRange(ListDC);


            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            var comp = Dbcontext.UserMasters.Where(m => m.UID == objModel.CreatedBy).SingleOrDefault();
            int? cid = comp.CompID;
            objModel.comp = cid;

            return View(objModel);
        }
        [HttpPost]
        public ActionResult DEdit(DeliveryModel model)
        {
            DeliveryService objService = new DeliveryService();
            int uid = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
                objService.Update(model);
                TempData["Msg"] = "Updated successfully.";
            }
            return RedirectToAction("Delivery");
        }
    }
}
