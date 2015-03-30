using System.Linq;
using System.Web.Http;
using System.Web.Security;
using SE.DSP.Foundation.Infrastructure.Interception;
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
            var model = AutoMapper.Mapper.Map<UserModel>(user);

            if (user != null)
            {
                var cookie = model.GetCookieContent();
                FormsAuthentication.SetAuthCookie(cookie, true);
            }
            
            return model;
        }
 
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/splogin")]
        public UserModel SpLogin([FromBody]LoginModel login)
        {
            var user = this.userService.SpLogin(login.SpDomain, login.UserName, login.Password);
            var model = AutoMapper.Mapper.Map<UserModel>(user);

            if (user != null)
            {
                var cookie = model.GetCookieContent();
                FormsAuthentication.SetAuthCookie(cookie, true);
            }

            return model;
        }

        [HttpPost]
        [Route("api/user/create")]
        public UserModel Create([FromBody]UserModel user)
        {
            user.SpId = ServiceContext.CurrentUser.SPId;
            user.UserType = 400001;
            user.UserTypeName = "SP管理员";

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

        [HttpPost]
        [Route("api/user/{userId}/usercustomer/save")]
        public UserCustomerModel[] SaveUserCustomer([FromBody]SaveUserCustomerModel saveUserCustomer)
        {
            var result = this.userService.SaveUserCustomer(saveUserCustomer.UserId, saveUserCustomer.UserCustomers.Select(u => AutoMapper.Mapper.Map<UserCustomerDto>(u)).ToArray());

            return result.Select(u => AutoMapper.Mapper.Map<UserCustomerModel>(u)).ToArray();
        }

        [HttpGet]
        [Route("api/user/{userId}/usercustomers")]
        public UserCustomerModel[] GetUserCustomerByUserId(long userId)
        {
            var result = this.userService.GetUserCustomerByUserId(userId);

            return result.Select(u => AutoMapper.Mapper.Map<UserCustomerModel>(u)).ToArray();
        }
    }
}
