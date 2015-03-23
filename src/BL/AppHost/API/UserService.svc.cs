using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Entity = SE.DSP.Foundation.Infrastructure.BE.Entities;

namespace SE.DSP.Pop.BL.AppHost.API
{
    public class UserService : IUserService
    {
        private readonly SE.DSP.Foundation.API.IUserService userServiceProxy;

        public UserService()
        {
            this.userServiceProxy = ServiceProxy<SE.DSP.Foundation.API.IUserService>.GetClient("IUserService.EndPoint");
        }

        public UserDto Login(string userName, string password)
        {
            Entity.UserDto dto;

            var result = this.userServiceProxy.ValidateLogin(userName, password, out dto);

            if(!result)
            {
                //todo:
                //throw login failed exception
            }

            return AutoMapper.Mapper.Map<UserDto>(dto);
        }

        public UserDto SpLogin(string spdomain, string userName, string password)
        {
            Entity.UserDto dto;

            var result = this.userServiceProxy.ValidateSpLogin(spdomain, userName, password, out dto);

            if (!result)
            {
                //todo:
                //throw login failed exception
            }

            return AutoMapper.Mapper.Map<UserDto>(dto);
        }
 
    }
}