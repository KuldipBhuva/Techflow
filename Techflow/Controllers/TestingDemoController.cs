using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Techflow.Controllers
{
    public class TestingDemoController : Controller
    {
        //
        // GET: /TestingDemo/

        public ActionResult Index()
        {
            return View();
        }
        public class DDLOptions
        {
            public int Id { get; set; }
            public string CityName { get; set; }
            public int stateID { get; set; }
            public string StateName { get; set; }

        }
        [HttpGet]
        public JsonResult CityList()
        {
            List<DDLOptions> obj = new List<DDLOptions>()  
            {  
                new DDLOptions {Id=1, CityName="Latur" ,stateID = 2 , StateName="Gujarat"},  
                new DDLOptions {Id=2, CityName="Pune" ,stateID = 4 , StateName="Maharastra"},  
                new DDLOptions {Id=4, CityName="Mumbai"  ,stateID = 6 , StateName="Delhi"},  
                new DDLOptions {Id=5, CityName="New Delhi"  ,stateID = 8 , StateName="New"}  
            }.ToList();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
