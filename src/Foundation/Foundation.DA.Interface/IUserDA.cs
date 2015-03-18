using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure.BE.Entities;


namespace SE.DSP.Foundation.DA.Interface
{
    public interface IUserDA
    {
        long CreateUser(UserEntity user, ConnectionOption option = null);
        void UpdateUser(UserEntity user, ConnectionOption option = null);
        void DeleteUser(UserFilter filter, ConnectionOption option = null);
        UserEntity[] RetrieveUsers(UserFilter filter, ConnectionOption option = null);

        void CreateUserCustomer(UserCustomerEntity[] entity, ConnectionOption option = null);
        void DeleteUserCustomer(UserCustomerFilter filter, ConnectionOption option = null);
        UserCustomerEntity[] RetrieveUserCustomers(UserCustomerFilter filter, ConnectionOption option = null);

        void CreateUserRole(UserRoleEntity[] entities, ConnectionOption option = null);
        void DeleteUserRole(RoleFilter filter, ConnectionOption option = null);

        UserEntity RetrieveUserById(long userId);
        //UserEntity[] RetrieveAllUsers();
        //void UpdateUserCustomer(UserCustomerEntity userCustomerEntity);
        //UserCustomerEntity RetrieveUserCustomerByUser(long userId);
    }
}
