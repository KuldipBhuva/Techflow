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
    public class APILoginController : ApiController
    {
        //
        // GET: /APILogin/

        public UserModelAPI Get(string Username, string pass)
        {
            UserModelAPI objModel = new UserModelAPI();
            UserService objService = new UserService();
            List<UserModelAPI> lstuser = new List<UserModelAPI>();

            lstuser = objService.getUserAPI();
            objModel.ListUser = new List<UserModelAPI>();
            objModel.ListUser.AddRange(lstuser);

            return objModel.ListUser.Where(m => m.UserName == Username && m.Password == pass && m.Status == 1).SingleOrDefault();
         
        }

    }
}
