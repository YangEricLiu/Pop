using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.Contract
{
    public interface IHierarchyAdministratorRepository : IRepository<HierarchyAdministrator, long>
    {
        HierarchyAdministrator[] GetByHierarchyId(long hierarchyId);

        void DeleteAdministratorByHierarchyId(IUnitOfWork unitOfWork, long hierarchyId);
    }
}
