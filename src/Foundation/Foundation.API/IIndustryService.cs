
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using System.ServiceModel;

namespace SE.DSP.Foundation.API
{
    [ServiceContract]
    public interface IIndustryService
    {
        [OperationContract]
        IndustryDto GetIndustryById(long industryId);

        [OperationContract]
        IndustryDto[] GetAllIndustries(bool onlyLeaf, bool includeRoot);
    }
}