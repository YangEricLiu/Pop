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
    public class CustomerService : ICustomerService
    { 
        private readonly IOssRepository ossRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly SE.DSP.Foundation.API.IUserService userService;
        private readonly IHierarchyRepository hierarchyRepository;

        public CustomerService()
        {
            this.ossRepository = IocHelper.Container.Resolve<IOssRepository>();
            this.customerRepository = IocHelper.Container.Resolve<ICustomerRepository>();
            this.hierarchyRepository = IocHelper.Container.Resolve<IHierarchyRepository>();
            this.userService = ServiceProxy<SE.DSP.Foundation.API.IUserService>.GetClient("IUserService.EndPoint");
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
         
        public DataContract.CustomerListItemDto[] GetCustomersByUserId(long userId)
        {
            var user = this.userService.GetUserById(userId);

            var filter = new UserCustomerFilter
            {
                WholeCustomer = user.CustomerIds.Length == 1 && user.CustomerIds[0] == 0
            };

            if (!filter.WholeCustomer.HasValue || !filter.WholeCustomer.Value)
            {
                filter.CustomerIds = user.CustomerIds;
            }

            var userCustomerResult = this.userService.RetrieveUserCustomers(filter);

            var customerIds = userCustomerResult.Select(uc => uc.CustomerId).ToArray();

            var hierarchyMap = this.hierarchyRepository.GetByIds(customerIds).ToDictionary(hr => hr.Id);

            var customers = this.customerRepository.GetByIds(customerIds);

            return customers.Select(c => new DataContract.CustomerListItemDto
                {
                    CustomerId = c.HierarchyId,
                    CustomerName = hierarchyMap.ContainsKey(c.HierarchyId) ? hierarchyMap[c.HierarchyId].Name : string.Empty,
                    StartTime = c.StartTime
                }).ToArray();
        }
    }
}