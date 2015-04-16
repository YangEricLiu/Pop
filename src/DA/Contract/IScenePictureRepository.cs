using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.Contract
{
    public interface IScenePictureRepository : IRepository<ScenePicture, long>
    {
        ScenePicture[] GetScenePictureByHierarchyId(long hierarchyId);

        void DeleteByHierarchyId(IUnitOfWork unitOfWork, long hierarchyId);
    }
}
