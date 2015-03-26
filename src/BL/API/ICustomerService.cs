using System.ServiceModel;
using SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.API
{
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        DataContract.LogoDto GetLogoById(long id);

        [OperationContract]
        DataContract.CustomerListItemDto[] GetCustomersByUserId(long userId);

        [OperationContract]
        DataContract.CustomerDto CreateCustomer(DataContract.CustomerDto customer);

        [OperationContract]
        DataContract.CustomerDto UpdateCustomer(DataContract.CustomerDto customer);
    }
}
