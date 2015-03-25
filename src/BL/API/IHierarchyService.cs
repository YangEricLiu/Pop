﻿using System.ServiceModel;
using SE.DSP.Pop.BL.API.DataContract;

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
