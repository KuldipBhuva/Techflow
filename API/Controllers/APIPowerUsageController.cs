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
    public class APIPowerUsageController : ApiController
    {
        //
        // GET: /APIPowerUsage/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public IEnumerable<PowerUsageModel> Get(int id)
        {
            PowerUsageModel objModel = new PowerUsageModel();
            PowerUsageService objService = new PowerUsageService();
            List<PowerUsageModel> lstticker = new List<PowerUsageModel>();

            lstticker = objService.getPUData(id);
            objModel.ListPowerData = new List<PowerUsageModel>();
            objModel.ListPowerData.AddRange(lstticker);
            return objModel.ListPowerData;
        }

    }
}
