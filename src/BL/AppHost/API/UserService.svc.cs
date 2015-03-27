using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;
using DataContract = SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.AppHost.API
{
    public class UserService : IUserService
    {
        private readonly SE.DSP.Foundation.API.IUserService userServiceProxy;
        private readonly ILogoRepository logoRepository;
        private readonly IOssRepository ossRepository;
        private readonly IHierarchyRepository hierarchyRepository;
        private readonly IUserCustomerRepository userCustomerRepository;
        private readonly IUnitOfWorkProvider unitOfWorkProvider;
        private readonly ICustomerRepository customerRepository;

        public UserService()
        {
            this.userServiceProxy = ServiceProxy<SE.DSP.Foundation.API.IUserService>.GetClient("IUserService.EndPoint");
            this.hierarchyRepository = IocHelper.Container.Resolve<IHierarchyRepository>();
            this.logoRepository = IocHelper.Container.Resolve<ILogoRepository>();
            this.ossRepository = IocHelper.Container.Resolve<IOssRepository>();
            this.userCustomerRepository = IocHelper.Container.Resolve<IUserCustomerRepository>();
            this.unitOfWorkProvider = IocHelper.Container.Resolve<IUnitOfWorkProvider>();
            this.customerRepository = IocHelper.Container.Resolve<ICustomerRepository>();
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

        public DataContract.UserCustomerDto[] SaveUserCustomer(long userId, DataContract.UserCustomerDto[] userCustomers)
        {
            var oldUserCustomers = this.userCustomerRepository.GetUserCustomersByUserId(userId);

            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                if (oldUserCustomers != null)
                {
                    this.userCustomerRepository.DeleteByUserId(unitOfWork, userId);
                } 

                var entities = userCustomers.Where(u => u.IsAuthorized).Select(u => new UserCustomer(userId, u.HierarchyId, !u.WholeCustomer, "SchneiderElectricChina")).ToArray();

                entities = this.userCustomerRepository.AddMany(unitOfWork, entities).ToArray();

                unitOfWork.Commit();

                return userCustomers;
            }
        }

        public DataContract.UserCustomerDto[] GetCustomerByUserId(long userId)
        {
            ////todo: get the sp id from context
            var allCustomers = this.customerRepository.GetBySpId(1);

            var hierarchyMap = this.hierarchyRepository.GetByIds(allCustomers.Select(c => c.HierarchyId).ToArray()).ToDictionary(hr => hr.Id);

            var userCustomers = this.userCustomerRepository.GetUserCustomersByUserId(userId);

            var result = new List<DataContract.UserCustomerDto>();

            foreach (var customer in allCustomers)
            {
                if (userCustomers.Length == 1 && userCustomers[0].HasAllCustomer())
                {
                    result.Add(new DataContract.UserCustomerDto
                    {
                        WholeCustomer = true,
                        HierarchyId = customer.HierarchyId,
                        IsAuthorized = true,
                        CustomerName = hierarchyMap.ContainsKey(customer.HierarchyId) ? hierarchyMap[customer.HierarchyId].Name : string.Empty
                    });
                }
                else
                {
                    var uc = userCustomers.SingleOrDefault(c => c.HierarchyId == customer.HierarchyId);

                    result.Add(new DataContract.UserCustomerDto
                        {
                            WholeCustomer = false,
                            HierarchyId = customer.HierarchyId,
                            IsAuthorized = uc != null,
                            CustomerName = hierarchyMap.ContainsKey(customer.HierarchyId) ? hierarchyMap[customer.HierarchyId].Name : string.Empty
                        });
                }
            }

            return result.ToArray();
        }

        public DataContract.UserDto CreateUser(DataContract.UserDto user)
        {
            var userApiDto = AutoMapper.Mapper.Map<UserDto>(user);

            userApiDto = this.userServiceProxy.CreateUser(userApiDto);

            return AutoMapper.Mapper.Map<DataContract.UserDto>(userApiDto);
        }

        public DataContract.UserDto UpdateUser(DataContract.UserDto user)
        {
            var userApiDto = AutoMapper.Mapper.Map<UserDto>(user);

            userApiDto = this.userServiceProxy.ModifyUser(userApiDto);

            return AutoMapper.Mapper.Map<DataContract.UserDto>(userApiDto);
        }

        public void DeleteUser(long userId)
        {
            this.userServiceProxy.DeleteUser(new Foundation.Infrastructure.BaseClass.DtoBase
                {
                    Id = userId
                });
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

            dtoResult.Customers = userCustomerResult.Select(uc => new DataContract.UserPreviligedCustomerDto
            {
                CustomerId = uc.CustomerId,
                CustomerLogoId = logos.ContainsKey(uc.CustomerId) ? new long?(logos[uc.CustomerId].Id) : null,
                CustomerName = hierarchyMap.ContainsKey(uc.CustomerId) ? hierarchyMap[uc.CustomerId].Name : string.Empty
            }).ToArray();

            return dtoResult;
        }
    }
}