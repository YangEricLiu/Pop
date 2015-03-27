using System;
using System.Collections.Generic;
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
        private readonly IHierarchyRepository hierarchyRepository;

        public UserService()
        {
            this.userServiceProxy = ServiceProxy<SE.DSP.Foundation.API.IUserService>.GetClient("IUserService.EndPoint");
            this.hierarchyRepository = IocHelper.Container.Resolve<IHierarchyRepository>();
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

        public DataContract.UserDto[] GetUserBySpId(long spId)
        {
            var users = this.userServiceProxy.GetUsersByFilter(new UserFilterDto
            {
                SpId = spId
            });

            return users.Select(u => AutoMapper.Mapper.Map<DataContract.UserDto>(u)).ToArray();
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

            var customerIds = userCustomerResult.Select(uc => uc.CustomerId).ToArray();

            var logos = this.logoRepository.GetLogosByHierarchyIds(customerIds).ToDictionary(lg => lg.HierarchyId);

            var hierarchyMap = this.hierarchyRepository.GetByIds(customerIds).ToDictionary(hr => hr.Id);

            var dtoResult = AutoMapper.Mapper.Map<DataContract.UserDto>(user);

            dtoResult.Customers = userCustomerResult.Select(uc => new DataContract.UserCustomerDto
            {
                CustomerId = uc.CustomerId,
                CustomerLogoId = logos.ContainsKey(uc.CustomerId) ? new long?(logos[uc.CustomerId].Id) : null,
                CustomerName = hierarchyMap.ContainsKey(uc.CustomerId) ? hierarchyMap[uc.CustomerId].Name : string.Empty
            }).ToArray();

            return dtoResult;
        }
    }
}