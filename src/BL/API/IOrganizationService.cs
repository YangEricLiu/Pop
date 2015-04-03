using System.ServiceModel;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.API
{
    [ServiceContract]
    public interface IOrganizationService
    {
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
