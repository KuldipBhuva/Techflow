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
    public class APIUserController : ApiController
    {
        //
        // GET: /APIUser/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public IEnumerable<UserModelAPI> Get(int id)
        {
            UserModelAPI objModel = new UserModelAPI();
            UserService objService = new UserService();
            List<UserModelAPI> lstuser = new List<UserModelAPI>();

            lstuser = objService.getUserAPI();
            objModel.ListUser = new List<UserModelAPI>();
            objModel.ListUser.AddRange(lstuser);
            return objModel.ListUser.Where(m=>m.UID == id ).ToList();

        }


        public UserModelAPI Post(UserModelAPI Task)
        {
            UserService objService = new UserService();
            var rsp = objService.InsertUserAPI(Task);
            return rsp;
        }


        public UserModelAPI Put(UserModelAPI Task)
        {
            UserService objService = new UserService();
            var rsp = objService.UpdateAPI(Task);
            return rsp;
        }

    }
}
