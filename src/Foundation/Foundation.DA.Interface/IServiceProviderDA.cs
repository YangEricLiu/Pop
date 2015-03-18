using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure.BE.Entities;


namespace SE.DSP.Foundation.DA.Interface
{
    public interface IServiceProviderDA
    {
        long CreateServiceProvider(ServiceProviderEntity entity);
        void UpdateServiceProvider(ServiceProviderEntity entity);
        void SetStatus(long[] spids, EntityStatus status, LastUpdateInfo update);
        ServiceProviderEntity[] RetrieveServiceProviders(ServiceProviderFilter filter);
    }
}
