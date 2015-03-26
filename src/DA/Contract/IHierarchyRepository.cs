using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.Contract
{
    public interface IHierarchyRepository : IRepository<Hierarchy, long>
    {
        Hierarchy[] GetByParentId(long parentId);

        Hierarchy[] GetByIds(long[] ids);

        long RetrieveSiblingHierarchyCountByCodeUnderParentCustomer(long hierarchyId, string hierarchyCode, long customerId);

        long RetrieveChildHierarchyCountByCodeUnderParentCustomer(string hierarchyCode, long customerId);

        long RetrieveAncestorAndSelfOrganizationCount(long organizationId);

        long RetrieveSiblingHierarchyCountByNameUnderParentCustomer(long hierarchyId, string hierarchyName, long parentCustomerId);

        long RetrieveChildHierarchyCountByNameUnderParentCustomer(string hierarchyName, long parentCustomerId);

        long RetrieveSiblingHierarchyCountByNameUnderParentHierarchy(long hierarchyId, string hierarchyName, long parentHierarchyId);

        long RetrieveChildHierarchyCountByNameUnderParentHierarchy(string hierarchyName, long parentHierarchyId);
    }
}
