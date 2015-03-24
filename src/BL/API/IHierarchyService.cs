using SE.DSP.Pop.BL.API.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.BL.API
{
    [ServiceContract]
    public interface IHierarchyService
    {
        [OperationContract]
        HierarchyDto GetHierarchyTree(long rootId);

        [OperationContract]
        HierarchyDto CreateHierarchy(HierarchyDto hierarchy);

        [OperationContract]
        void DeleteHierarchy(long hierarchyId, bool isRecursive);

        [OperationContract]
        void UpdateHierarchy(HierarchyDto hierarchy);
    }
}
