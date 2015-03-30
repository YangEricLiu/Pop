using System.ServiceModel;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
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

        [OperationContract]
        OrganizationDto GetOrganizationById(long hierarchyId);

        [OperationContract]
        OrganizationDto CreateOrganization(OrganizationDto organization);

        [OperationContract]
        OrganizationDto UpdateOrganization(OrganizationDto organization);

        [OperationContract]
        void DeleteOrganization(long hierarchyId);
    }
}
