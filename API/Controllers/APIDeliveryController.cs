using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Models.ViewModel;
using Services;
using Models;


namespace API.Controllers
{
    public class APIDeliveryController : ApiController
    {
        //
        // GET: /APIDelivery/

        public IEnumerable<DeliveryModel> Get(int id)
        {
            DeliveryModel objModel = new DeliveryModel();
            DeliveryService objService = new DeliveryService();
            List<DeliveryModel> lstuser = new List<DeliveryModel>();
            lstuser = objService.getDeliveryReq(id);
            return lstuser;

        }
    }
}
