using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;


namespace SE.DSP.Foundation.DA.API
{ 
    using SE.DSP.Foundation.Infrastructure.BaseClass;
    using SE.DSP.Foundation.Infrastructure.Structure;
   
    using SE.DSP.Foundation.Infrastructure.Utils;
    using SE.DSP.Foundation.Infrastructure.BE.Entities;
    using SE.DSP.Foundation.DA.Interface;

    public class UserAPI : DAAPIBase, IPersistor<UserEntity>
    {
        #region DI
        private IUserDA _userDA;
        private IUserDA UserDA
        {
            get
            {
                return _userDA ?? (_userDA = IocHelper.Container.Resolve<IUserDA>());
            }
        }

        protected override void RegisterType()
        {
            IocHelper.Container.RegisterInstanceSingleton((IUserDA)DAFactory.CreateDA(typeof(IUserDA)));
        }
        #endregion

        public long CreateUser(UserEntity user, ConnectionOption option = null)
        {
            return UserDA.CreateUser(user, option);
        }
        public void UpdateUser(UserEntity user, ConnectionOption option = null)
        {
            UserDA.UpdateUser(user, option);
        }
        public void DeleteUser(UserFilter filter, ConnectionOption option = null)
        {
            UserDA.DeleteUser(filter, option);
        }
        public void CreateUserCustomer(UserCustomerEntity[] entity, ConnectionOption option = null)
        {
            UserDA.CreateUserCustomer(entity, option);
        }
        public void DeleteUserCustomer(UserCustomerFilter filter, ConnectionOption option = null)
        {
            UserDA.DeleteUserCustomer(filter, option);
        }
        public UserEntity RetrieveUserById(long userId, ConnectionOption option = null)
        {
            var entities = UserDA.RetrieveUsers(new UserFilter { UserIds = new[] { userId }, SpId=-1 }, option);
            return entities.Length == 0 ? null : entities[0];
        }
        public UserEntity[] RetrieveUsers(UserFilter filter, ConnectionOption option = null)
        {
            return UserDA.RetrieveUsers(filter, option);
        }
        public UserCustomerEntity[] RetrieveUserCustomers(UserCustomerFilter filter, ConnectionOption option = null)
        {
            return UserDA.RetrieveUserCustomers(filter, option);
        }
        public void CreateUserRole(UserRoleEntity[] entities, ConnectionOption option = null)
        {
            UserDA.CreateUserRole(entities, option);
        }
        public void DeleteUserRole(RoleFilter filter, ConnectionOption option = null)
        {
            UserDA.DeleteUserRole(filter, option);
        }

        public long Create(UserEntity entity)
        {
            return CreateUser(entity);
        }

        public void Update(UserEntity entity)
        {
            UpdateUser(entity);
        }

        public void Delete(long id)
        {
            DeleteUser(new UserFilter { UserIds = new[] { id } });
        }

        public UserEntity RetrieveById(long id)
        {
            return RetrieveUserById(id);
        }

    }
}
