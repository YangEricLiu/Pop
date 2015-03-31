using System;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.Infrastructure.Utils;

namespace SE.DSP.Pop.BL.AppHost.Common.Ioc
{
    public class IocInstanceProvider : IInstanceProvider
    {
        private readonly Type serviceType;
        private readonly IUnityContainer unityContainer;

        public IocInstanceProvider(Type type)
        {
            this.serviceType = type;
            this.unityContainer = IocHelper.Container;
        }

        public object GetInstance(System.ServiceModel.InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            return this.unityContainer.Resolve(this.serviceType);
        }

        public object GetInstance(System.ServiceModel.InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(System.ServiceModel.InstanceContext instanceContext, object instance)
        {
            if (instance is IDisposable)
            {
                ((IDisposable)instance).Dispose();
            }
        }
    }
}