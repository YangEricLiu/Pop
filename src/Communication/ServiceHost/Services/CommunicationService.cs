using SE.DSP.Pop.Communication.ServiceHost.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Communication.ServiceHost.Services
{
    public class CommunicationService : ICommunicationService
    {
        public double Add(double n1, double n2)
        {
            return n1 + n2;
        }
    }
}
