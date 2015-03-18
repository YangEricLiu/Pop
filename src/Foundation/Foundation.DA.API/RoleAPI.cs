using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace SE.DSP.Foundation.DA.API
{
    using SE.DSP.Foundation.Infrastructure.BaseClass;
    using SE.DSP.Foundation.Infrastructure.Utils;
    using SE.DSP.Foundation.Infrastructure.Structure;
    using SE.DSP.Foundation.Infrastructure.BE.Entities;
    using SE.DSP.Foundation.DA.Interface;
    
    public class RoleAPI : DAAPIBase, IPersistor<RoleEntity>
    {
        #region DI
        private IRoleDA _roleDA;
        private IRoleDA RoleDA
        {
            get
            {
                return _roleDA ?? (_roleDA = IocHelper.Container.Resolve<IRoleDA>());
            }
        }

        protected override void RegisterType()
        {
            IocHelper.Container.RegisterInstanceSingleton((IRoleDA)DAFactory.CreateDA(typeof(IRoleDA)));
        }
        #endregion


        public long CreateRole(RoleEntity entity)
        {
            return RoleDA.CreateRole(entity);
        }
        public void UpdateRole(RoleEntity entity)
        {
            RoleDA.UpdateRole(entity);
        }
        public void DeleteRole(long[] roleIds)
        {
            RoleDA.DeleteRole(roleIds);
        }
        public RoleEntity[] RetrieveRolesByFilter(RoleFilter filter, ConnectionOption connOp = null)
        {
            return RoleDA.RetrieveRolesByFilter(filter,connOp);
        }

        public long Create(RoleEntity entity)
        {
            return CreateRole(entity);
        }

        public void Update(RoleEntity entity)
        {
            UpdateRole(entity);
        }

        public void Delete(long id)
        {
            DeleteRole(new [] {id});
        }

        public RoleEntity RetrieveById(long id)
        {
            var roles =RetrieveRolesByFilter(new RoleFilter {RoleIds = new [] {id}});
            return roles.Length == 0 ? null : roles[0];
        }
    }
}
