using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Communication.ServiceHost.API
{
    [ServiceContract]
    public interface ICommunicationService
    {
        [OperationContract]
        double Add(double n1, double n2);

        [OperationContract]
        void HierarchyChanged();
    }
}
