using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.AppHost.Common.Ioc;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;
using DataContract = SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.AppHost.API
{
    [IocServiceBehavior]
    public class UserService : IUserService
    {
        private readonly SE.DSP.Foundation.API.IUserService userServiceProxy;
        private readonly ILogoRepository logoRepository;
        private readonly IOssRepository ossRepository;
        private readonly IHierarchyRepository hierarchyRepository;
        private readonly IUserCustomerRepository userCustomerRepository;
        private readonly IUnitOfWorkProvider unitOfWorkProvider;
        private readonly ICustomerRepository customerRepository;

        public UserService(SE.DSP.Foundation.API.IUserService userServiceProxy, IHierarchyRepository hierarchyRepository, ILogoRepository logoRepository, IOssRepository ossRepository, IUserCustomerRepository userCustomerRepository, IUnitOfWorkProvider unitOfWorkProvider, ICustomerRepository customerRepository)
        {
            this.userServiceProxy = userServiceProxy;
            this.hierarchyRepository = hierarchyRepository;
            this.logoRepository = logoRepository;
            this.ossRepository = ossRepository;
            this.userCustomerRepository = userCustomerRepository;
            this.unitOfWorkProvider = unitOfWorkProvider;
            this.customerRepository = customerRepository;
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

        public DataContract.UserCustomerDto[] GetUserCustomerByUserId(long userId)
        {
            var allCustomers = this.customerRepository.GetBySpId(ServiceContext.CurrentUser.SPId);

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

        public DataContract.UserDto GetUserById(long userId)
        {
            var user = this.userServiceProxy.GetUserById(userId);

            return this.FillCustomerForUser(user);
        }

        private DataContract.UserDto FillCustomerForUser(UserDto user)
        {
            var allCustomers = this.customerRepository.GetBySpId(user.SpId);

            var customerIds = allCustomers.Select(uc => uc.HierarchyId).ToArray();

            var hierarchyMap = this.hierarchyRepository.GetByIds(customerIds).ToDictionary(hr => hr.Id);

            var logos = this.logoRepository.GetLogosByHierarchyIds(customerIds).ToDictionary(lg => lg.HierarchyId);

            var userCustomers = this.userCustomerRepository.GetUserCustomersByUserId(user.Id.Value);

            var dtoResult = AutoMapper.Mapper.Map<DataContract.UserDto>(user);

            var upc = new List<DataContract.UserPreviligedCustomerDto>();

            foreach (var customer in allCustomers)
            {
                if (userCustomers.Length == 1 && userCustomers[0].HasAllCustomer())
                {
                    upc.Add(new DataContract.UserPreviligedCustomerDto
                    {
                        CustomerId = customer.HierarchyId,
                        CustomerLogoId = logos.ContainsKey(customer.HierarchyId) ? logos[customer.HierarchyId].Id : 0,
                        CustomerName = hierarchyMap.ContainsKey(customer.HierarchyId) ? hierarchyMap[customer.HierarchyId].Name : string.Empty
                    });
                }
                else if (user.CustomerIds.Contains(customer.HierarchyId))
                {
                    upc.Add(new DataContract.UserPreviligedCustomerDto
                    {
                        CustomerId = customer.HierarchyId,
                        CustomerLogoId = logos.ContainsKey(customer.HierarchyId) ? logos[customer.HierarchyId].Id : 0,
                        CustomerName = hierarchyMap.ContainsKey(customer.HierarchyId) ? hierarchyMap[customer.HierarchyId].Name : string.Empty
                    });
                }
            }

            dtoResult.Customers = upc.ToArray();
 
            return dtoResult;
        }
    }
}