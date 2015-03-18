using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure;
using System;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using System.ServiceModel;


namespace SE.DSP.Foundation.API
{
    [ServiceContract]
    public interface IAccessControlService
    {

        [OperationContract]
        RoleDto CreateRole(RoleDto role);

        [OperationContract]
        RoleDto ModifyRole(RoleDto role);

        [OperationContract]
        void DeleteRole(DtoBase role);

        [OperationContract]
        RoleDto[] GetRolesByFilter(UserRoleFilterDto filter);

        [OperationContract]
        RoleEntity[] RetrieveRolesByFilter(RoleFilter filter);

        [OperationContract]
        DataPrivilegeDto SetDataPrivilege(DataPrivilegeDto dto);

        [OperationContract]
        void DeleteDataPrivileges(DataPrivilegeFilter filter);

        [OperationContract]
        void CreateDataPrivilegeEntities(DataPrivilegeEntity[] dataPrivileges);

        [OperationContract]
        DataPrivilegeEntity[] RetrieveDataPrivilegeEntities(DataPrivilegeFilter filter);

        [OperationContract]
        UserDto[] GetUsersByPrivilegeItem(UserPrivilegedFilter filter);

        [OperationContract]
        String[] GetFuncPrivilegesByFilter(UserRoleFilterDto filter);

        [OperationContract]
        bool ValidateUser(string userName, string password, out UserDto currentUser);

        [OperationContract]
        bool ValidateSpUser(string domain, string userName, string password, out UserDto currentUser);
        
        [OperationContract]
        bool HasDataAuth(long userId, DataAuthType privilegeType, long privilegeItemId);

        [OperationContract]
        bool HasFuncAuth(long userId, String[] privilegeCodes);

        [OperationContract]
        String[] GetPrivilegesByFilter(UserRoleFilterDto filter);

        [OperationContract]
        void UpdateDataprivileges(long[] privileges);

        [OperationContract]
        String[] GetPrivileges();
    }
}