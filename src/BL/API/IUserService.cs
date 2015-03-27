﻿using System.ServiceModel;
using SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.API
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        UserDto Login(string userName, string password);

        [OperationContract]
        UserDto SpLogin(string spdomain, string userName, string password);

        [OperationContract]
        UserDto[] GetUserBySpId(long spId);

        [OperationContract]
        UserCustomerDto[] SaveUserCustomer(long userId, UserCustomerDto[] userCustomers);

        [OperationContract]
        UserCustomerDto[] GetCustomerByUserId(long userId);
    }
}
