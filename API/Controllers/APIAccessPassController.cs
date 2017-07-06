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
    public class APIAccessPassController : ApiController
    {
        //
        // GET: /APIAccessPass/

        public IEnumerable<AccessPassModel> Get(int id)
        {
            AccessPassModel objModel = new AccessPassModel();
            AccessPassService objService = new AccessPassService();
            List<AccessPassModel> lstuser = new List<AccessPassModel>();
            lstuser = objService.getAPData(id);
            return lstuser;

        }

    }
}
