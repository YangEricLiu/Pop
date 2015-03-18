using SE.DSP.Foundation.Infrastructure.BE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SE.DSP.Foundation.DA.Interface
{
    public interface IRolePrivilegeDA
    {
        void CreateRolePrivilege(RolePrivilegeEntity[] entities);
        void DeleteRolePrivilege(RoleFilter filter);
        RolePrivilegeEntity[] RetrieveRolePrivilege(RoleFilter filter);
        String[] RetrievePrivileges();
    }
}
