using System.Linq;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Foundation.DataAccess.Entity;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;
using DataContract = SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.AppHost.API
{
    public class CustomerService : ICustomerService
    { 
        private readonly IOssRepository ossRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly SE.DSP.Foundation.API.IUserService userService;
        private readonly IHierarchyRepository hierarchyRepository;
        private readonly IHierarchyAdministratorRepository hierarchyAdministratorRepository;
        private readonly IUnitOfWorkProvider unitOfWorkProvider;
        private readonly ILogoRepository logoRepository;

        public CustomerService()
        {
            this.ossRepository = IocHelper.Container.Resolve<IOssRepository>();
            this.customerRepository = IocHelper.Container.Resolve<ICustomerRepository>();
            this.hierarchyRepository = IocHelper.Container.Resolve<IHierarchyRepository>();
            this.unitOfWorkProvider = IocHelper.Container.Resolve<IUnitOfWorkProvider>();
            this.hierarchyAdministratorRepository = IocHelper.Container.Resolve<IHierarchyAdministratorRepository>();
            this.logoRepository = IocHelper.Container.Resolve<ILogoRepository>();
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

        public DataContract.CustomerDto CreateCustomer(DataContract.CustomerDto customer)
        {
            var hierarchy = new Hierarchy(customer.CustomerName);

            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                hierarchy = this.hierarchyRepository.Add(unitOfWork, hierarchy);

                var customerEntity = new Customer(hierarchy.Id, customer.Address, customer.StartTime);

                customerEntity = this.customerRepository.Add(unitOfWork, customerEntity);

                customer.HierarchyId = hierarchy.Id;

                if (customer.Administrators != null)
                {
                    var hierarchyAdmins = customer.Administrators.Select(ad => new HierarchyAdministrator(customerEntity.HierarchyId, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToList();

                    customer.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hierarchyAdmins).Select(ha => AutoMapper.Mapper.Map<DataContract.HierarchyAdministratorDto>(ha)).ToArray();
                }

                if (customer.Logo != null)
                {
                    var logo = this.logoRepository.Add(unitOfWork, new Logo(hierarchy.Id));

                    customer.Logo.Id = logo.Id;

                    this.ossRepository.Add(new OssObject(string.Format("img-logo-{0}", logo.Id), customer.Logo.Logo));
                }

                unitOfWork.Commit();
            }

            return customer;
        }

        public DataContract.CustomerDto UpdateCustomer(DataContract.CustomerDto customer)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchy = this.hierarchyRepository.GetById(customer.HierarchyId.Value);

                hierarchy.Name = customer.CustomerName;

                this.hierarchyRepository.Update(unitOfWork, hierarchy);

                var customerEntity = this.customerRepository.GetById(customer.HierarchyId.Value);

                customerEntity.StartTime = customer.StartTime;

                this.customerRepository.Update(unitOfWork, customerEntity);

                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, customer.HierarchyId.Value);

                if (customer.Administrators != null)
                {
                    var hierarchyAdmins = customer.Administrators.Select(ad => new HierarchyAdministrator(customer.HierarchyId.Value, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToList();

                    customer.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hierarchyAdmins).Select(ha => AutoMapper.Mapper.Map<DataContract.HierarchyAdministratorDto>(ha)).ToArray();
                }

                this.logoRepository.DeleteByHierarchyId(unitOfWork, customer.HierarchyId.Value);

                if (customer.Logo != null)
                {
                    var logo = this.logoRepository.Add(unitOfWork, new Logo(hierarchy.Id));

                    customer.Logo.Id = logo.Id;

                    this.ossRepository.Add(new OssObject(string.Format("img-logo-{0}", logo.Id), customer.Logo.Logo));
                }

                unitOfWork.Commit();
            }

            return customer;
        }

        public DataContract.HierarchyAdministratorDto[] SaveHierarchyAdministrators(DataContract.HierarchyAdministratorDto[] administrators)
        {
            var hierarchyId = administrators.First().HierarchyId.Value;

            var hierarchyAdmins = this.hierarchyAdministratorRepository.GetByHierarchyId(hierarchyId);

            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                if (hierarchyAdmins != null && hierarchyAdmins.Length > 0)
                {
                    this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyId);
                }

                var hierarchyAdminList = administrators.Select(ad => new HierarchyAdministrator(ad.HierarchyId.Value, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToList();

                hierarchyAdmins = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hierarchyAdminList).ToArray();

                unitOfWork.Commit();

                return hierarchyAdmins.Select(ha => AutoMapper.Mapper.Map<DataContract.HierarchyAdministratorDto>(ha)).ToArray();
            }
        }

        public void DeleteCustomer(long customerid)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                this.logoRepository.DeleteByHierarchyId(unitOfWork, customerid);
                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, customerid);
                this.customerRepository.Delete(unitOfWork, customerid);
                this.hierarchyRepository.Delete(unitOfWork, customerid);

                unitOfWork.Commit();
            }
        }
    }
}