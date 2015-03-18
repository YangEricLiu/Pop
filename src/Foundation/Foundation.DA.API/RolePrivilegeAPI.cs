using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SE.DSP.Foundation.Infrastructure;

using Microsoft.Practices.Unity;

using SE.DSP.Foundation.Infrastructure.Structure;

namespace SE.DSP.Foundation.DA.API
{
    using SE.DSP.Foundation.Infrastructure.BaseClass;
    using SE.DSP.Foundation.Infrastructure.Utils;
    using SE.DSP.Foundation.Infrastructure.BE.Entities;
    using SE.DSP.Foundation.DA.Interface;
    
    public class RolePrivilegeAPI : DAAPIBase
    {
        #region DI

        private IRolePrivilegeDA _rolePrivilegeDa;
        private IRolePrivilegeDA RolePrivilegeDA
        {
            get
            {
                return this._rolePrivilegeDa ?? (this._rolePrivilegeDa = IocHelper.Container.Resolve<IRolePrivilegeDA>());
            }
        }

        /// <summary>
        /// Register types those are needed by calendar data access api into IoC container.
        /// </summary>
        protected override void RegisterType()
        {
            IocHelper.Container.RegisterInstanceSingleton((IRolePrivilegeDA)DAFactory.CreateDA(typeof(IRolePrivilegeDA)));
        }
        #endregion

        public void CreateRolePrivilege(RolePrivilegeEntity[] entities)
        {
            RolePrivilegeDA.CreateRolePrivilege(entities);
        }
        public void DeleteRolePrivilege(RoleFilter filter)
        {
            RolePrivilegeDA.DeleteRolePrivilege(filter);
        }
        public RolePrivilegeEntity[] RetrieveRolePrivilege(RoleFilter filter)
        {
            return RolePrivilegeDA.RetrieveRolePrivilege(filter);
        }

        public String[] RetrievePrivileges()
        {
            return RolePrivilegeDA.RetrievePrivileges();
        }
    }
}
