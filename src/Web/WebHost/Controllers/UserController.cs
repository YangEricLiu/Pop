using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {

        private readonly IUserService userService;

        public UserController()
        {
            this.userService = SE.DSP.Foundation.Web.Wcf.ServiceProxy<IUserService>.GetClient("IUserService.EndPoint");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/login")]
        public UserModel Login([FromBody]LoginModel login)
        {
            var user = this.userService.Login(login.UserName, login.Password);

            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Name, true);
            }

            return AutoMapper.Mapper.Map<UserModel>(user);
        }
 
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/splogin")]
        public UserModel SpLogin([FromBody]LoginModel login)
        {
            var user = this.userService.SpLogin(login.SpDomain, login.UserName, login.Password);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Name, true);
            }

            return AutoMapper.Mapper.Map<UserModel>(user);
        }
    }
}

