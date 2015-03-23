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
    public interface IUserService
    {
        [OperationContract]
        UserDto Login(string userName, string password);

        [OperationContract]
        UserDto SpLogin(string spdomain, string userName, string password);
    }
}
