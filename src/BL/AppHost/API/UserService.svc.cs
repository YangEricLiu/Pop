using System.Linq;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.Contract;
using DataContract = SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.AppHost.API
{
    public class UserService : IUserService
    {
        private readonly SE.DSP.Foundation.API.IUserService userServiceProxy;
        private readonly ILogoRepository logoRepository;

        public UserService()
        {
            this.userServiceProxy = ServiceProxy<SE.DSP.Foundation.API.IUserService>.GetClient("IUserService.EndPoint");
            this.logoRepository = IocHelper.Container.Resolve<ILogoRepository>();
        }

        public DataContract.UserDto Login(string userName, string password)
        { 
            var result = this.userServiceProxy.ValidateLogin(userName, password);

            return this.FillCustomerForUser(result);
        }

        public DataContract.UserDto SpLogin(string spdomain, string userName, string password)
        {
            var result = this.userServiceProxy.ValidateSpLogin(spdomain, userName, password);

            return this.FillCustomerForUser(result);
        }

        private DataContract.UserDto FillCustomerForUser(UserDto user)
        {
            var userCustomerResult = this.userServiceProxy.RetrieveUserCustomers(new UserCustomerFilter
            {
                CustomerIds = user.CustomerIds,
                WholeCustomer = user.CustomerIds.Length == 1 && user.CustomerIds[0] == 0
            });

            var logos = this.logoRepository.GetLogosByHierarchyIds(userCustomerResult.Select(uc => uc.CustomerId).ToArray()).ToDictionary(lg => lg.HierarchyId);

            var dtoResult = AutoMapper.Mapper.Map<DataContract.UserDto>(user);

            dtoResult.Customers = userCustomerResult.Select(uc => new DataContract.CustomerDto
                {
                    CustomerId = uc.CustomerId,
                    CustomerLogoId = logos.ContainsKey(uc.CustomerId) ? new long?(logos[uc.CustomerId].Id) : null,
                    CustomerName = string.Empty
                }).ToArray();

            return dtoResult;
        }
    }
}