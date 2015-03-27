using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

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

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(login.UserName, true);
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
                FormsAuthentication.SetAuthCookie(login.UserName, true);
            }

            return AutoMapper.Mapper.Map<UserModel>(user);
        }

        [HttpPost]
        [Route("api/user/create")]
        public UserModel Create([FromBody]UserModel user)
        {
            var userDto = this.userService.CreateUser(AutoMapper.Mapper.Map<UserDto>(user));

            return AutoMapper.Mapper.Map<UserModel>(userDto);
        }

        [HttpPost]
        [Route("api/user/update")]
        public UserModel Update([FromBody]UserModel user)
        {
            var userDto = this.userService.UpdateUser(AutoMapper.Mapper.Map<UserDto>(user));

            return AutoMapper.Mapper.Map<UserModel>(userDto);
        }

        [HttpPost]
        [Route("api/user/delete/{userId}")]
        public void Delete(long userId)
        {
            this.userService.DeleteUser(userId);
        }

        [HttpGet]
        [Route("api/sp/{spid}/users")]
        public UserModel[] GetUserBySpId(long spId)
        {
            var users = this.userService.GetUserBySpId(spId);

            return users.Select(u => AutoMapper.Mapper.Map<UserModel>(u)).ToArray();
        }
    }
}
