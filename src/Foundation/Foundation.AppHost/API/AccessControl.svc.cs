using SE.DSP.Foundation.API;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Practices.Unity;

namespace SE.DSP.Foundation.AppHost.API
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AccessControl : ServiceBase, IAccessControlService
    {
        #region DI

        private IUserService _userBL;
        public IUserService UserBL
        {
            get { return _userBL ?? (_userBL = IocHelper.Container.Resolve<IUserService>()); }
            set { _userBL = value; }
        }

        private IRoleDA _roleApi;
        public IRoleDA RoleAPI
        {
            get { return _roleApi ?? (_roleApi = IocHelper.Container.Resolve<IRoleDA>()); }
            set { _roleApi = value; }
        }
        private IRolePrivilegeDA _rolePrivilegeAPI;
        public IRolePrivilegeDA RolePrivilegeAPI
        {
            get { return _rolePrivilegeAPI ?? (_rolePrivilegeAPI = IocHelper.Container.Resolve<IRolePrivilegeDA>()); }
            set { _rolePrivilegeAPI = value; }
        }
        private IDataPrivilegeDA _dataPrivilegeAPI;
        public IDataPrivilegeDA DataPrivilegeAPI
        {
            get { return _dataPrivilegeAPI ?? (_dataPrivilegeAPI = IocHelper.Container.Resolve<IDataPrivilegeDA>()); }
            set { _dataPrivilegeAPI = value; }
        }

        #endregion

        public RoleDto CreateRole(RoleDto dto)
        {
            if (dto == null) return null;

            var datetime = DateTime.UtcNow;
            var username = ServiceContext.CurrentUser.Name;
            var spid = ServiceContext.CurrentUser.SPId;
            dto.SpId = spid;

            // Put the logical into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateRepeatableRead())
            {
                var roles = RoleAPI.RetrieveRolesByFilter(new RoleFilter { Name = dto.Name, SpId = spid });
                if (roles.Length > 0) ThrowBusinessLogicException(AccessControlErrorCode.RoleNameIsDuplicated);

                var roleId = RoleAPI.CreateRole(new RoleEntity { Name = dto.Name, UpdateTime = datetime, UpdateUser = username, SpId = spid });
                var arr = dto.PrivilegeCodes.Select(code => new RolePrivilegeEntity
                {
                    RoleId = roleId,
                    PrivilegeCode = code,
                    UpdateTime = datetime,
                    UpdateUser = username
                }).ToArray();
                RolePrivilegeAPI.CreateRolePrivilege(arr);

                roles = RoleAPI.RetrieveRolesByFilter(new RoleFilter { RoleIds = new[] { roleId }, SpId = spid });
                dto.Id = roleId;
                dto.Version = roles[0].Version;

                ts.Complete();
            }
            return dto;
        }

        public RoleDto ModifyRole(RoleDto dto)
        {
            if (dto == null) return null;

            var datetime = DateTime.UtcNow;
            var username = ServiceContext.CurrentUser.Name;

            var arr = dto.PrivilegeCodes.Select(code => new RolePrivilegeEntity
            {
                RoleId = dto.Id.Value,
                PrivilegeCode = code,
                UpdateTime = datetime,
                UpdateUser = username
            }).ToArray();

            if (dto.SpId == 0)
            {
                dto.SpId = ServiceContext.CurrentUser.SPId;
            }

            var userRoleFilter = new RoleFilter { RoleIds = new[] { dto.Id.Value }, SpId = dto.SpId };

            // Put the logical into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateRepeatableRead())
            {
                var roles = RoleAPI.RetrieveRolesByFilter(userRoleFilter);
                if (roles == null || roles.Length == 0) ThrowConcurrentException(AccessControlErrorCode.RoleDoesNotExist);
                if (roles[0].Version != dto.Version) ThrowConcurrentException(AccessControlErrorCode.RoleIsExpired);

                var spid = roles[0].SpId;
                dto.SpId = spid;

                roles = RoleAPI.RetrieveRolesByFilter(new RoleFilter { Name = dto.Name, ExcludeId = dto.Id, SpId = spid });
                if (roles.Length > 0) ThrowBusinessLogicException(AccessControlErrorCode.RoleNameIsDuplicated);

                RoleAPI.UpdateRole(new RoleEntity { Id = dto.Id.Value, Name = dto.Name, UpdateTime = datetime, UpdateUser = username, SpId = spid });

                RolePrivilegeAPI.DeleteRolePrivilege(userRoleFilter);
                RolePrivilegeAPI.CreateRolePrivilege(arr);

                roles = RoleAPI.RetrieveRolesByFilter(userRoleFilter);
                dto.Version = roles[0].Version;

                ts.Complete();
            }
            return dto;
        }

        public void DeleteRole(DtoBase dto)
        {
            var userRoleFilter = new RoleFilter { RoleIds = new[] { dto.Id.Value }, SpId = ServiceContext.CurrentUser.SPId };
            // Put the logical into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateRepeatableRead())
            {
                var roles = RoleAPI.RetrieveRolesByFilter(userRoleFilter);
                if (roles == null || roles.Length == 0) return;
                if (roles[0].Version != dto.Version) ThrowConcurrentException(AccessControlErrorCode.RoleIsExpired);
                var users = UserBL.RetrieveUserEntitiesByFilter(new UserFilter { RoleIds = new long[] { dto.Id.Value }, SpId = ServiceContext.CurrentUser.SPId });
                if (users.Length > 0) ThrowBusinessLogicException(AccessControlErrorCode.UserExistsAnagistDeletion);

                RolePrivilegeAPI.DeleteRolePrivilege(userRoleFilter);
                var roleIds = new[] { dto.Id.Value };
                UserBL.DeleteUserRole(new RoleFilter { RoleIds = roleIds, SpId = ServiceContext.CurrentUser.SPId });
                RoleAPI.DeleteRole(new[] { dto.Id.Value });

                ts.Complete();
            }
        }

        public void DeleteDataPrivileges(DataPrivilegeFilter filter)
        {
            DataPrivilegeAPI.DeleteDataPrivileges(filter);
        }

        public RoleDto[] GetRolesByFilter(UserRoleFilterDto filter)
        {
            RoleFilter filter1 = UserTranslator.UserRoleFilterDto2UserRoleFilter(filter);
            filter1.SpId = ServiceContext.CurrentUser.SPId;

            var roleEntities = RoleAPI.RetrieveRolesByFilter(filter1);
            var roleIds = roleEntities.Select(x => x.Id.Value).ToArray();
            filter1.RoleIds = roleIds;
            var rolePrivileges = RolePrivilegeAPI.RetrieveRolePrivilege(filter1).OrderBy(p => p.RoleId).ToArray();
            var roledtos = roleEntities.Select(RoleTranslator.RoleEntity2RoleDto).OrderBy(p => p.Id).ToArray();

            var index = 0;
            foreach (var roledto in roledtos)
            {
                var privileges = new List<String>();
                for (; index < rolePrivileges.Length; index++)
                {
                    if (rolePrivileges[index].RoleId != roledto.Id.Value) break;
                    privileges.Add(rolePrivileges[index].PrivilegeCode);
                }
                roledto.PrivilegeCodes = privileges.ToArray();
            }
            return roledtos;
        }
        public RoleEntity[] RetrieveRolesByFilter(RoleFilter filter)
        {
            return RoleAPI.RetrieveRolesByFilter(filter);
        }
        public DataPrivilegeDto SetDataPrivilege(DataPrivilegeDto dto)
        {
            if (dto == null) return null;

            var loguser = ServiceContext.CurrentUser.Name;
            var logtime = DateTime.UtcNow;

            var dataprivileges = new List<DataPrivilegeEntity>();
            var userCustomerEntities = new List<UserCustomerEntity>();

            if (dto.WholeSystem) userCustomerEntities.Add(new UserCustomerEntity { UserId = dto.UserId, CustomerId = 0, UpdateTime = logtime, UpdateUser = loguser });
            else if (dto.Privileges != null && dto.Privileges.Length > 0)
            {
                foreach (var privilege in dto.Privileges)
                {
                    if (privilege.HierarchyIds != null && privilege.HierarchyIds.Length > 0 && !privilege.WholeCustomer)
                    {
                        dataprivileges.AddRange(privilege.HierarchyIds.Select(p => new DataPrivilegeEntity { UserId = dto.UserId, PrivilegeType = dto.PrivilegeType, PrivilegeItemId = p, UpdateTime = logtime, UpdateUser = loguser }));
                    }
                    userCustomerEntities.Add(new UserCustomerEntity { UpdateUser = loguser, UpdateTime = logtime, UserId = dto.UserId, CustomerId = privilege.CustomerId, WholeCustomer = privilege.WholeCustomer });
                }
            }

            var customerIds = userCustomerEntities.Select(p => p.CustomerId).ToArray();
            var hierarchyIds = dataprivileges.Select(p => p.PrivilegeItemId.Value).ToArray();

            // Put the logical into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateRepeatableRead())
            {
                var users = UserBL.RetrieveUserEntitiesByFilter(new UserFilter { UserIds = new[] { dto.UserId } });

                if (users == null || users.Length == 0) ThrowConcurrentException(AccessControlErrorCode.UserDoesNotExist);

                if (customerIds.Length > 0 && customerIds[0] != 0)
                {
                    var customers = DataPrivilegeAPI.RetrieveCustomerIdsByPrivilegeItem(customerIds);
                    if (customers.Length != customerIds.Length) ThrowConcurrentException(AccessControlErrorCode.SomeCustomersAreDeleted);

                    if (hierarchyIds.Length > 0)
                    {
                        var checkCustomerIds = DataPrivilegeAPI.RetrieveCustomerIdsByPrivilegeItem(hierarchyIds);
                        if (checkCustomerIds.Length != hierarchyIds.Length) ThrowConcurrentException(AccessControlErrorCode.SomeHierarchiesAreDeleted);
                    }
                }

                var ucs = UserBL.RetrieveUserCustomers(new UserCustomerFilter { UserIds = new[] { dto.UserId } });
                if (ucs != null && ucs.Length != 0)
                {
                    var version = ucs.Max(p => p.Version.Value);
                    if (dto.Version != version) ThrowConcurrentException(AccessControlErrorCode.DataPrivilegeIsExpired);
                }

                UserBL.DeleteUserCustomer(new UserCustomerFilter { UserIds = new[] { dto.UserId } });
                UserBL.CreateUserCustomer(userCustomerEntities.ToArray());

                DataPrivilegeAPI.DeleteDataPrivileges(new DataPrivilegeFilter { UserIds = new[] { dto.UserId } });
                DataPrivilegeAPI.CreateDataPrivileges(dataprivileges.ToArray());
 
                ucs = UserBL.RetrieveUserCustomers(new UserCustomerFilter { UserIds = new[] { dto.UserId } });
                dto.Version = ucs.Max(p => p.Version);

                ts.Complete();
            }
            return dto;
        }
        public DataPrivilegeEntity[] RetrieveDataPrivilegeEntities(DataPrivilegeFilter filter)
        {
            return DataPrivilegeAPI.RetrieveDataPrivileges(filter);
        }

        public void CreateDataPrivilegeEntities(DataPrivilegeEntity[] dataPrivileges)
        {
            DataPrivilegeAPI.CreateDataPrivileges(dataPrivileges);
        }

        public String[] GetFuncPrivilegesByFilter(UserRoleFilterDto filter)
        {
            var entities = RolePrivilegeAPI.RetrieveRolePrivilege(new RoleFilter { UserIds = filter.UserIds, RoleIds = filter.RoleIds });
            return entities.Select(p => p.PrivilegeCode).ToArray();
        }

        public UserDto[] GetUsersByPrivilegeItem(UserPrivilegedFilter filter)
        {
            var items = DataPrivilegeAPI.RetrieveDataPrivileges(new DataPrivilegeFilter { PrivilegeType = filter.PrivilegeType, PrivilegeItemIds = new[] { filter.PrivileteItemId } });

            var customerIds = DataPrivilegeAPI.RetrieveCustomerIdsByPrivilegeItem(new long[] { filter.PrivileteItemId });

            if (customerIds.Length == 0) ThrowConcurrentException(AccessControlErrorCode.HierarchyHasBeenDeleted);

            var customers = new[] { 0, customerIds[0] };
            var userCustomersWhole = UserBL.RetrieveUserCustomers(new UserCustomerFilter { CustomerIds = customers });

            var userIds = new List<long>();
            userIds.AddRange(items.Where(p => p.UserId != ServiceContext.CurrentUser.Id).Select(p => p.UserId));
            userIds.AddRange(userCustomersWhole.Where(p => p.UserId != ServiceContext.CurrentUser.Id && (p.CustomerId == 0 || p.WholeCustomer)).Select(p => p.UserId));
            var userIdArr = userIds.ToArray();
            if (userIdArr.Length == 0) return new UserDto[0];

            var users = UserBL.GetUsersByFilter(new UserFilterDto { UserIds = userIdArr, DemoStatus = 0 });
            var roles = RoleAPI.RetrieveRolesByFilter(new RoleFilter());

            foreach (var user in users)
            {
                var usertype = roles.FirstOrDefault(p => p.Id == user.UserType);
                if (usertype != null) user.UserTypeName = usertype.Name;
            }

            return users;
        }

        public bool HasDataAuth(long userId, DataAuthType privilegeType, long privilegeItemId)
        {
            if (userId == -1) return true;

            var wholeCustomers = GetWholePrivilegedCustomersByUser(userId);
            if (wholeCustomers.Length != 0)
            {
                if (wholeCustomers[0] == 0) return true;
                var customerIds = DataPrivilegeAPI.RetrieveCustomerIdsByPrivilegeItem(new long[] { privilegeItemId });
                if (customerIds == null || customerIds.Length == 0) return true;
                if (wholeCustomers.Contains(customerIds[0])) return true;
                return true;
            }

            var dataAuths = DataPrivilegeAPI.RetrieveDataPrivileges(new DataPrivilegeFilter { UserIds = new[] { userId }, });
            return dataAuths.Any(p => p.PrivilegeType == privilegeType && p.PrivilegeItemId.Value == privilegeItemId);
        }

        public bool HasFuncAuth(long userId, string[] privilegeCodes)
        {
            if (userId == -1) return true;
            var user = UserBL.GetUserById(userId);
            if (user.UserType == -1) return true;

            var entities = RolePrivilegeAPI.RetrieveRolePrivilege(new RoleFilter { UserIds = new[] { userId } });
            return entities != null && entities.Any(entity => privilegeCodes.Any(code => entity.PrivilegeCode == code));
        }


        protected void ThrowConcurrentException(AccessControlErrorCode error)
        {
            throw new ConcurrentException(Layer.BL, Module.AccessControl, Convert.ToInt32(error));
        }
        protected void ThrowBusinessLogicException(AccessControlErrorCode error)
        {
            throw new BusinessLogicException(Layer.BL, Module.AccessControl, Convert.ToInt32(error));
        }


        public void UpdateDataprivileges(long[] privileges)
        {
            this.DataPrivilegeAPI.UpdateDataprivileges(privileges);
        }

        public string[] GetPrivilegesByFilter(UserRoleFilterDto filter)
        {
            return this.GetFuncPrivilegesByFilter(filter);
        }
        private long[] GetWholePrivilegedCustomersByUser(long userId)
        {
            var usercustomers = UserBL.RetrieveUserCustomers(new UserCustomerFilter { UserIds = new[] { userId } });

            if (usercustomers == null) return new long[0];
            if (usercustomers.Length == 0) return new long[0];
            if (usercustomers.Length == 1 && usercustomers[0].CustomerId == 0) return new long[] { 0 };
            return usercustomers.Where(p => p.WholeCustomer).Select(p => p.CustomerId).ToArray();
        }

        public String[] GetPrivileges()
        {
            return RolePrivilegeAPI.RetrievePrivileges();
        }
    }
}
