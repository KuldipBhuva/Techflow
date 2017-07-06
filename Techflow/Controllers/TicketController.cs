using System;
using System.Collections.Generic;
using System.IO;
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
    public class TicketController : Controller
    {
        //
        // GET: /Ticket/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index()
        {
            TicketService objService = new TicketService();
            TicketModel objModel = new TicketModel();
            List<TicketModel> objList = new List<TicketModel>();

            int uid = 0;
            int rid = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
                rid = Convert.ToInt32(Session["Role"].ToString());
            }
            objList = objService.getTicket(uid, rid);
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(objList);

            List<TicketTypeModel> ListTT = new List<TicketTypeModel>();
            ListTT = objService.getTicketType();
            objModel.ListTT = new List<TicketTypeModel>();
            objModel.ListTT.AddRange(ListTT);

            List<TicketStatusModel> ListTS = new List<TicketStatusModel>();
            ListTS = objService.getTicketStatus();
            objModel.ListTS = new List<TicketStatusModel>();
            objModel.ListTS.AddRange(ListTS);

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
        public ActionResult Index(HttpPostedFileBase[] files, TicketModel model, TicketTranModel model1)
        {
            TicketService objService = new TicketService();
            TicketModel objModel = new TicketModel();
            model.TicketStatusID = 1;
            model.IsActive = true;
            int uid = 0;
            int tid = 0;

            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
                if (model.CreatedBy == null)
                {
                    model.CreatedBy = uid;
                }

                //if (model.TicketTypeID == 1)
                //{
                //    var TList = Dbcontext.TicketMasters.Where(m => m.TicketNo.Contains("REQ")).Max(m => m.TicketNo).ToList();

                //}
                model.CreatedDate = System.DateTime.Now;
                if (model.TicketTypeID == 1)
                {
                    model.Prefix = "REQ";
                    model1.Prefix = "REQ";
                }
                else if (model.TicketTypeID == 2)
                {
                    model.Prefix = "INC";
                    model1.Prefix = "INC";
                }
                else
                {
                    model.Prefix = "CHG";
                    model1.Prefix = "CHG";
                }
                objModel = objService.GetLastID();
                var dateTimeNow = DateTime.Now;
                var dateOnlyString = dateTimeNow.ToShortDateString();
                //DateTime dt = DateTime.ParseExact(objModel.CreatedDate.ToString(), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                //string createddt = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                DateTime dt = DateTime.Parse(objModel.CreatedDate.ToString());
                string createddt = dt.ToString("dd-MMM-yy", CultureInfo.InvariantCulture);
                if (createddt == dateOnlyString)
                {

                    model.TicketID = objModel.TicketID + 1;

                }
                else
                {
                    model.TicketID = objModel.TicketID + 38;

                }
                
                tid = objService.InsertTicket(model);
                TempData["Msg"] = "New Ticket Created.";

                if (files != null)
                {
                    //string k = objService.Getid();
                    //int s = Convert.ToInt32(k);
                    //string strtext = "Customer";
                    foreach (HttpPostedFileBase file in files)
                    {
                        if (file != null)
                        {
                            string filename = System.IO.Path.GetFileName(file.FileName);
                            string folderPath = Server.MapPath("~/Attachment/") + model.TicketID;
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
                            model1.TicketID = tid;
                            model1.Comment = model.Description;
                            model1.FileURL = Path.Combine("/Attachment/" + model.TicketID + "/", fileNewName);
                            model1.FileName = Path.GetFileNameWithoutExtension(file.FileName);
                            if (model.CreatedBy == null)
                            {
                                model1.CreatedBy = uid;
                            }
                            else
                            {
                                model1.CreatedBy = model.CreatedBy;
                            }
                            model1.CreatedDate = System.DateTime.Now;
                            //model1.ReplayBy = uid;
                            //model1.ReplayDate = System.DateTime.Now;
                            objService.InsertTicketTran(model1);
                            TempData["Msg"] = "New Ticket Created.";
                        }
                        else
                        {
                            model1.TicketID = tid;
                            model1.Comment = model.Description;
                            if (model.CreatedBy == null)
                            {
                                model1.CreatedBy = uid;
                            }
                            else
                            {
                                model1.CreatedBy = model.CreatedBy;
                            }
                            model1.CreatedDate = System.DateTime.Now;
                            //model1.ReplayBy = uid;
                            //model1.ReplayDate = System.DateTime.Now;
                            objService.InsertTicketTran(model1);
                            TempData["Msg"] = "New Ticket Created.";
                        }
                    }
                }

                try
                {
                    var user = Dbcontext.UserMasters.Where(m => m.UID == model.CreatedBy).SingleOrDefault();
                    var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == user.CompID).SingleOrDefault();
                    //var fromAddress = new MailAddress("newtech_infosoft@yahoo.com", comp.Name);
                    var fromAddress = new MailAddress("support@techflow.com.au", comp.Name);
                    var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                    //var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                    const string fromPassword = "sUpport##11";
                    var tt = Dbcontext.TicketTypeMasters.Where(a => a.TicketTypeID == model.TicketTypeID).SingleOrDefault();

                    string subject = "New Ticket-" + model.Prefix + "" + tid;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html>");
                    sb.Append("<Body>");
                    sb.Append("<table><tr><td><b>Dear Techflow Administrator,</b></td></tr><tr><td><br/><b>Created By : </b></td><td><br/>" + user.Title + ' ' + user.FirstName + ' ' + user.LastName + "</td></tr><tr><td><b>Company : </b></td><td>" + comp.Name + "</td></tr><tr><td><b>Ticket ID : </b></td><td>" + model.Prefix + "" + tid + "</td></tr><tr><td><b>Subject : </b></td><td>" + model.Subject + "</td></tr><tr><td><b>Ticket Type : </b></td><td>" + tt.TicketType + "</td></tr><tr><td><b>Ticket Priority : </b></td><td>" + "P" + model.Priority + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + user.FirstName + ' ' + user.LastName + "</td></tr></table>");
                    sb.Append("</Body>");
                    sb.Append("</html>");

                    string body = sb.ToString();

                    var smtp = new SmtpClient
                    {
                        //Host = "smtp.mail.yahoo.com",
                        //Port = 587,
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
                        foreach (HttpPostedFileBase file in files)
                        {
                            if (file != null)
                            {
                                string fileName = Path.GetFileName(file.FileName);
                                message.Attachments.Add(new Attachment(file.InputStream, fileName));
                            }
                        }
                        message.IsBodyHtml = true;
                        //message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Distributer Order"));
                        //message.CC.Add(new MailAddress(Convert.ToString("dhrumilshah@newtechinfosoft.in"), "Dhrumil"));
                        smtp.Send(message);
                        TempData["Msg"] = "New Ticket Created.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "New Ticket Created but Email not sent.";
                }



            }
            return RedirectToAction("Index");
        }
        public ActionResult Replay(int id, int uid)
        {
            TicketService objService = new TicketService();
            TicketModel objModel = new TicketModel();
            objModel = objService.getTicketByID(id);

            UserService objUService = new UserService();
            CompanyService objCompSer = new CompanyService();

            objModel.UserDetails = objUService.getByID(uid);
            objModel.CompDetails = objCompSer.getByID(Convert.ToInt32(objModel.UserDetails.CompID));

            int rid = 0;
            if (Session["Role"] != null)
            {
                rid = Convert.ToInt32(Session["Role"].ToString());
            }
            List<TicketTranModel> ListTicketTran = new List<TicketTranModel>();

            ListTicketTran = objService.getTicketTran(uid, id, rid);
            objModel.ListTicketTran = new List<TicketTranModel>();
            objModel.ListTicketTran.AddRange(ListTicketTran);

            List<TicketStatusModel> objTStatus = new List<TicketStatusModel>();
            objTStatus = objService.getTicketStatus();
            objModel.ListTstatus = new List<TicketStatusModel>();
            objModel.ListTstatus.AddRange(objTStatus);


            objModel.Quotedetails = objService.getQuoteByTIcketID(id);
            objModel.Deldetails = objService.getDelByTIcketID(id);
            objModel.Accessdetails = objService.getAccessByTIcketID(id);


            //List<TicketAttachmentModel> ListTicketAtt = new List<TicketAttachmentModel>();
            //ListTicketAtt = objService.getTicketAttachments(uid);
            //objModel.ListTA = new List<TicketAttachmentModel>();
            //objModel.ListTA.AddRange(ListTicketAtt);

            TempData["uid"] = uid;
            TempData["tid"] = id;
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Replay(HttpPostedFileBase[] files, TicketTranModel model, TicketModel model1)
        {
            TicketService objService = new TicketService();
            int uid = 0;
            int rid = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
                rid = Convert.ToInt32(Session["Role"].ToString());
                TicketMaster tm = Dbcontext.TicketMasters.Where(m => m.TicketID == model1.TicketID).SingleOrDefault();
                tm.TicketStatusID = model1.TicketStatusID;
                tm.UpdatedDate = System.DateTime.Now;
                tm.UpdatedBy = uid;
                Dbcontext.SaveChanges();

                if (files != null)
                {
                    //string k = objService.Getid();
                    //int s = Convert.ToInt32(k);
                    //string strtext = "Customer";
                    foreach (HttpPostedFileBase file in files)
                    {
                        if (file != null)
                        {
                            string filename = System.IO.Path.GetFileName(file.FileName);
                            string folderPath = Server.MapPath("~/Attachment/") + model.TicketID;
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
                            model.FileURL = Path.Combine("/Attachment/" + model.TicketID + "/", fileNewName);
                            model.FileName = Path.GetFileNameWithoutExtension(file.FileName);
                            model.TicketID = model1.TicketID;
                            model.Prefix = model1.Prefix;
                            model.Comment = model1.Comment;
                            model.CreatedBy = model.CreatedBy;
                            model.CreatedDate = model.CreatedDate;
                            model.ReplayBy = uid;
                            model.ReplayDate = System.DateTime.Now;
                            objService.InsertTicketTran(model);
                            //objModel.BID = Convert.ToInt32(Session["BranchID"].ToString());
                            //objService.InsertTicketAttachment(model1);

                            //string filepathtosave = "Images/" + filename;
                            /*HERE WILL BE YOUR CODE TO SAVE THE FILE DETAIL IN DATA BASE*/
                        }
                        else if (model1.Comment != null)
                        {
                            model.TicketID = model1.TicketID;
                            model.Prefix = model1.Prefix;
                            model.Comment = model1.Comment;
                            model.CreatedBy = model.CreatedBy;
                            model.CreatedDate = model.CreatedDate;
                            model.ReplayBy = uid;
                            model.ReplayDate = System.DateTime.Now;
                            objService.InsertTicketTran(model);
                        }
                        else
                        {
                            TempData["AMsg"] = "Enter your message or Attach any files in attachment.";
                        }
                    }
                }
                if (model1.Comment != null || files[0] != null)
                {
                    try
                    {
                        if (rid != 1)
                        {
                            var user = Dbcontext.UserMasters.Where(m => m.UID == model.ReplayBy).SingleOrDefault();
                            var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == user.CompID).SingleOrDefault();
                            var ts = Dbcontext.TicketStatusMasters.Where(m => m.TicketStatusID == model1.TicketStatusID).SingleOrDefault();
                            var fromAddress = new MailAddress("support@techflow.com.au", comp.Name);
                            var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                            //var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                            const string fromPassword = "sUpport##11";

                            var ticket = Dbcontext.TicketMasters.Where(m => m.TicketID == model1.TicketID).SingleOrDefault();
                            string subject = model.Prefix + "" + model.TicketID;
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<html>");
                            sb.Append("<Body>");
                            sb.Append("<table><tr><td><b>Dear Techflow Administrator,</b></td></tr><tr><td><br/><b>Ticket ID : </b></td><td><br/>" + model.Prefix + "" + model.TicketID + "</td></tr><tr><td><b>Reply By : </b></td><td>" + user.Title + ' ' + user.FirstName + ' ' + user.LastName + "</td></tr><tr><td><b>Ticket Status : </b></td><td>" + ts.TicketStatus + "</td></tr><tr><td><b>Comment : </b></td><td>" + model.Comment + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + user.FirstName + ' ' + user.LastName + "</td></tr></table>");
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
                                foreach (HttpPostedFileBase file in files)
                                {
                                    if (file != null)
                                    {
                                        string fileName = Path.GetFileName(file.FileName);
                                        message.Attachments.Add(new Attachment(file.InputStream, fileName));
                                    }
                                }
                                message.IsBodyHtml = true;
                                //message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Distributer Order"));
                                //message.CC.Add(new MailAddress(Convert.ToString("dhrumilshah@newtechinfosoft.in"), "Dhrumil"));
                                //message.CC.Add(new MailAddress(Convert.ToString("info@sugamhealthcare.com"), "Info-Sugam Health Care"));
                                smtp.Send(message);
                                TempData["Msg"] = "Your Reply Successfully Sent.";
                            }
                        }
                        else
                        {
                            var user = Dbcontext.UserMasters.Where(m => m.UID == model1.CreatedBy).SingleOrDefault();
                            //var fromAddress = new MailAddress("newtech_infosoft@yahoo.com", "Techflow Administrator");
                            var fromAddress = new MailAddress("support@techflow.com.au", "Techflow Administrator");
                            //var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                            var toAddress = new MailAddress(user.Email, user.FirstName + " " + user.LastName);
                            const string fromPassword = "sUpport##11";

                            var ticket = Dbcontext.TicketMasters.Where(m => m.TicketID == model1.TicketID).SingleOrDefault();
                            string subject = model.Prefix + "" + model.TicketID;
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<html>");
                            sb.Append("<Body>");
                            sb.Append("<table><tr><td><b>Dear User,</b></td></tr><tr><td><br/><b>Ticket ID : </b></td><td><br/>" + model.Prefix + "" + model.TicketID + "</td></tr><tr><td><b>Reply By : </b></td><td>Techflow Administrator</td></tr><tr><td><b>Comment : </b></td><td>" + model.Comment + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>Techflow</td></tr></table>");
                            sb.Append("</Body>");
                            sb.Append("</html>");

                            string body = sb.ToString();

                            var smtp = new SmtpClient
                            {
                                //Host = "smtp.mail.yahoo.com",
                                //Port = 25,
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
                                foreach (HttpPostedFileBase file in files)
                                {
                                    if (file != null)
                                    {
                                        string fileName = Path.GetFileName(file.FileName);
                                        message.Attachments.Add(new Attachment(file.InputStream, fileName));
                                    }
                                }
                                message.IsBodyHtml = true;
                                //message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Distributer Order"));
                                //message.CC.Add(new MailAddress(Convert.ToString("dhrumilshah@newtechinfosoft.in"), "Dhrumil"));
                                //message.CC.Add(new MailAddress(Convert.ToString("info@sugamhealthcare.com"), "Info-Sugam Health Care"));
                                smtp.Send(message);
                                TempData["Msg"] = "Your Reply Successfully Sent.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Msg"] = "Reply was not sent. Please try again later.";
                    }
                }

            }
            return RedirectToAction("Replay", new { @id = TempData["tid"], @uid = TempData["uid"] });
        }
    }
}
