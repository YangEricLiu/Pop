using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure.BE.Entities; 


namespace SE.DSP.Foundation.DA.Interface
{
    public interface IRoleDA
    {
        long CreateRole(RoleEntity entity);
        void UpdateRole(RoleEntity entity);
        void DeleteRole(long[] roleIds);
        RoleEntity[] RetrieveRolesByFilter(RoleFilter filter, ConnectionOption connOp = null);
    }
}
