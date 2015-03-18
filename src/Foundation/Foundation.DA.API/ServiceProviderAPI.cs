
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;



using SE.DSP.Foundation.Infrastructure;


using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.DA.Interface;

namespace SE.DSP.Foundation.DA.API
{
    public class ServiceProviderAPI: DAAPIBase
    {
        #region DI
        private IServiceProviderDA _ServiceProviderDA;
        private IServiceProviderDA ServiceProviderDA
        {
            get
            {
                return this._ServiceProviderDA ?? (this._ServiceProviderDA = IocHelper.Container.Resolve<IServiceProviderDA>());
            }
        }

        protected override void RegisterType()
        {
            IocHelper.Container.RegisterInstanceSingleton((IServiceProviderDA)DAFactory.CreateDA(typeof(IServiceProviderDA)));
        }
        #endregion


        public long CreateServiceProvider(ServiceProviderEntity entity)
        {
            return ServiceProviderDA.CreateServiceProvider(entity);
        }
        public void UpdateServiceProvider(ServiceProviderEntity entity)
        {
            ServiceProviderDA.UpdateServiceProvider(entity);
        }
        public void SetStatus(long[] spids, EntityStatus status, LastUpdateInfo update)
        {
            ServiceProviderDA.SetStatus(spids, status, update);
        }
        public ServiceProviderEntity[] RetrieveServiceProviders(ServiceProviderFilter filter)
        {
            return ServiceProviderDA.RetrieveServiceProviders(filter);
        }
    }
}
