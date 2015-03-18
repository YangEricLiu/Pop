using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Constant;
using System;
using System.ServiceModel;
namespace SE.DSP.Foundation.API
{
    [ServiceContract]
    public interface IServiceProviderService
    {
        [OperationContract]
        ServiceProviderDto CreateServiceProvider(ServiceProviderDto dto);

        [OperationContract]
        ServiceProviderDto ModifyServiceProvider(ServiceProviderDto dto);

        [OperationContract]
        void DeleteServiceProvider(DtoBase dto);

        [OperationContract]
        ServiceProviderDto[] GetServiceProviders(ServiceProviderFilter filter);

        [OperationContract]
        long GetMaxServiceProviderId();

        [OperationContract]
        ServiceProviderCalcInfo[] GetServiceProviderCalcInfos();

        [OperationContract]
        ServiceProviderDto SendInitPassword(long spId);

        [OperationContract]
        ParentCalcStatus GetSpCalcStatus(long spId);

        [OperationContract]
        void UpdateServiceProvider(ServiceProviderEntity entity);
    }
}
    

