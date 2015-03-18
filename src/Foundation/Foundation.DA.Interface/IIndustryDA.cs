
using SE.DSP.Foundation.Infrastructure.BE.Entities;

namespace SE.DSP.Foundation.DA.Interface
{
    public interface IIndustryDA
    {
        IndustryEntity RetrieveIndustryById(long industryId);
        IndustryEntity[] RetrieveAllIndustries();
    }
}