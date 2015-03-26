using System.Linq;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.DataAccess;
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
        private readonly IOssRepository ossRepository;

        public UserService()
        {
            this.userServiceProxy = ServiceProxy<SE.DSP.Foundation.API.IUserService>.GetClient("IUserService.EndPoint");
            this.logoRepository = IocHelper.Container.Resolve<ILogoRepository>();
            this.ossRepository = IocHelper.Container.Resolve<IOssRepository>();
        }

        public DataContract.UserDto Login(string userName, string password)
        { 
            var result = this.userServiceProxy.ValidateLogin(userName, password);

            if (result == null)
            {
                return null;
            }

            return this.FillCustomerForUser(result);
        }

        public DataContract.UserDto SpLogin(string spdomain, string userName, string password)
        {
            var result = this.userServiceProxy.ValidateSpLogin(spdomain, userName, password);

            if (result == null)
            {
                return null;
            }

            return this.FillCustomerForUser(result);
        }

        public DataContract.LogoDto GetLogoById(long id)
        {
            var oss = this.ossRepository.GetById(string.Format("img-logo-{0}", id));

            return new DataContract.LogoDto
            {
                Id = id,
                Logo = oss.Content
            };
        }

        private DataContract.UserDto FillCustomerForUser(UserDto user)
        {
            var filter = new UserCustomerFilter
            {
                WholeCustomer = user.CustomerIds.Length == 1 && user.CustomerIds[0] == 0
            };

            if (!filter.WholeCustomer.HasValue || !filter.WholeCustomer.Value)
            {
                filter.CustomerIds = user.CustomerIds;
            }

            var userCustomerResult = this.userServiceProxy.RetrieveUserCustomers(filter);

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