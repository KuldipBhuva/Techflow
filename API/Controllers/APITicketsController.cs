using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Models.ViewModel;
using Services;

namespace API.Controllers
{
    public class APITicketsController : ApiController
    {
        //
        // GET: /APITickets/

        public IEnumerable<TicketModel> Get(int id)
        {
            TicketModel objModel = new TicketModel();
            TicketService objService = new TicketService();
            List<TicketModel> lstticker = new List<TicketModel>();
            int rid = 2;
            lstticker = objService.getTicket(id,rid);
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(lstticker);
            return objModel.ListTicket.Where(m => m.CreatedBy == id).ToList();
            

        }


        //public TicketModel Post(TicketModel model, TicketTranModel model1) 
        // {


        //}
    }
}
