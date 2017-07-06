using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Techflow.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection All)
        {
            string name = Convert.ToString(All["txtname"]);
            string subject = Convert.ToString(All["txtsub"]);
            string phone = Convert.ToString(All["txtphone"]);
            string email = Convert.ToString(All["txtemail"]);
            string msg = Convert.ToString(All["txtmessage"]);

            try
            {
                var fromAddress = new MailAddress("support@techflow.com.au", "Techflow");
                var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                //var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                const string fromPassword = "sUpport##11";

                //string subject = "Quotation Request";

                StringBuilder sb = new StringBuilder();
                sb.Append("<html>");
                sb.Append("<Body>");
                sb.Append("<table><tr><td><b>Dear Techflow Administrator,</b></td></tr><tr><td><br/><b>Name : </b></td><td><br/>" + name + "</td></tr><tr><td><b>Phone : </b></td><td>" + phone + "</td></tr><tr><td><b>Email : </b></td><td>" + email + "</td></tr><tr><td><b>Message : </b></td><td>" + msg + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + name + "</td></tr></table>");
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
                    smtp.Send(message);
                    return Content("<script language='javascript' type='text/javascript'>alert('Detail sent successfully.');</script>");
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Sorry!! Detail not sent, Try again.');</script>");
            }

        }
        public ActionResult NewsLetter(FormCollection All)
        {
            try
            {
                var fromAddress = new MailAddress("support@techflow.com.au", "Client Request");
                //var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                const string fromPassword = "sUpport##11";
                string email = Convert.ToString(All["email"]);
                string subject = "Newsletter Subscription";

                StringBuilder sb = new StringBuilder();
                sb.Append("<html>");
                sb.Append("<Body>");
                sb.Append("<table><tr><td><b>Dear Techflow Administrator,</b></td></tr><tr><td><br/>Please add my email id : <a href='mailto:'" + email + ">" + email + "</a> to your newsletter subscription.</td><tr><td><br/><b>Thanks.</b></td></tr></table>");
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
                    return Content("<script language='javascript' type='text/javascript'>alert('News Letter Successfully Subscribed.');</script>");
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Sorry!! News letter not subscribed, Something went wrong, Try again!!');</script>");
            }

            //return RedirectToAction("Home/Index");
        }
    }
}
