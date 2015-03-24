using SE.DSP.Foundation.DataAccess;
using SE.DSP.Pop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Contract
{
    public interface IHierarchyRepository : IRepository<Hierarchy, long>
    {
        Hierarchy[] GetByParentId(long parentId);
    }
}
