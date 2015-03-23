using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Web.Wcf
{
    public class ServiceProxy<TInterface> : ClientBase<TInterface>, IDisposable where TInterface : class
    {
        public delegate void ServiceProxyDelegate<T>(TInterface proxy);

        public ServiceProxy() : base(typeof(TInterface).ToString()) { }
        public ServiceProxy(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        public ServiceProxy(string endpointConfigurationName, string endpointAddress)
            : base(endpointConfigurationName, endpointAddress)
        {
        }

        protected override TInterface CreateChannel()
        {
            var facotry = new ChannelFactory<TInterface>(this.Endpoint);

            foreach (var op in facotry.Endpoint.Contract.Operations)
            {
                var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();

                if (dataContractBehavior != null)
                {
                    dataContractBehavior.MaxItemsInObjectGraph = 2147483647;
                }
            }
 
            return facotry.CreateChannel();
        }

        public TInterface Proxy { get { return this.Channel; } }

        public static TInterface GetClient(string endpointName)
        {
            return new ServiceProxy<TInterface>(endpointName).Proxy;            
        }

        public void Dispose()
        {
            if (this.State == CommunicationState.Faulted)
            {
                base.Abort();
            }
            else
            {
                try
                {
                    base.Close();
                }

                catch
                {
                    base.Abort();
                }
            }
        }
    }
}
