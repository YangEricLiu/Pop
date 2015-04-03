using System.Linq;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Foundation.DataAccess.Entity;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.BL.AppHost.Common.Ioc;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;
using SE.DSP.Pop.Entity.Enumeration;
using DataContract = SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.AppHost.API
{
    [IocServiceBehavior]
    public class CustomerService : BaseHierarchyService, ICustomerService
    { 
        private readonly IOssRepository ossRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly SE.DSP.Foundation.API.IUserService userService; 
        private readonly IHierarchyAdministratorRepository hierarchyAdministratorRepository; 
        private readonly ILogoRepository logoRepository;

        public CustomerService(
                          IOssRepository ossRepository,
                          ICustomerRepository customerRepository,
                          SE.DSP.Foundation.API.IUserService userService,
                          IHierarchyRepository hierarchyRepository,
                          IHierarchyAdministratorRepository hierarchyAdministratorRepository,
                          IUnitOfWorkProvider unitOfWorkProvider,
                          ILogoRepository logoRepository) : base(hierarchyRepository, unitOfWorkProvider)
        {
            this.ossRepository = ossRepository;
            this.customerRepository = customerRepository;          
            this.hierarchyAdministratorRepository = hierarchyAdministratorRepository;
            this.logoRepository = logoRepository;
            this.userService = userService;
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

            var hierarchyMap = this.HierarchyRepository.GetByIds(customerIds).ToDictionary(hr => hr.Id);

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
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchy = new Hierarchy(customer.CustomerName, customer.Code, HierarchyType.Customer);

                this.CreateHierarchy(unitOfWork, hierarchy);

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
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchy = this.HierarchyRepository.GetById(customer.HierarchyId.Value);

                hierarchy.Name = customer.CustomerName;
                hierarchy.Code = customer.Code;

                this.UpdateHierarchy(unitOfWork, hierarchy);

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

            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
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
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                this.logoRepository.DeleteByHierarchyId(unitOfWork, customerid);
                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, customerid);
                this.customerRepository.Delete(unitOfWork, customerid);
                this.DeleteHierarchy(unitOfWork, customerid, true);

                unitOfWork.Commit();
            }
        }

        public DataContract.CustomerDto GetCustomerById(long customerId)
        {
            var customer = this.customerRepository.GetById(customerId);

            var hierarchy = this.HierarchyRepository.GetById(customerId);

            var hierarchyAdministrators = this.hierarchyAdministratorRepository.GetByHierarchyId(customerId);

            var logo = this.logoRepository.GetLogosByHierarchyIds(new long[] { customerId });

            OssObject logoObject = null;

            if (logo.Length == 1)
            {
                logoObject = this.ossRepository.GetById(string.Format("img-logo-{0}", logo[0].Id));
            }

            return new DataContract.CustomerDto
            {
                Address = customer.Address,
                CustomerName = hierarchy.Name,
                Code = hierarchy.Code,
                Email = customer.Email,
                Manager = customer.Manager,
                StartTime = customer.StartTime,
                Telephone = customer.Telephone,
                HierarchyId = customer.HierarchyId,
                Logo = logoObject == null ? null : new LogoDto { HierarchyId = customer.HierarchyId, Id = logo[0].Id, Logo = logoObject.Content },
                Administrators = hierarchyAdministrators.Select(ha => AutoMapper.Mapper.Map<HierarchyAdministratorDto>(ha)).ToArray()
            };
        }
    }
}