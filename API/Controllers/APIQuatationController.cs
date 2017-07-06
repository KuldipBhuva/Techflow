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
    public class APIQuatationController : ApiController
    {
        //
        // GET: /APIQuatation/

        public IEnumerable<QuotationModel> Get(int id)
        {
            QuotationModel objModel = new QuotationModel();
            QuotationService objService = new QuotationService();
            List<QuotationModel> lstuser = new List<QuotationModel>();
            lstuser = objService.getQuotation(id);
            return lstuser;

        }

    }
}
