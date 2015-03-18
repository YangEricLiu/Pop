using Microsoft.Practices.Unity;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.DA.Interface;


namespace SE.DSP.Foundation.DA.API
{
    public class ZoneAPI : DAAPIBase
    {
        #region DI
        private IZoneDA _zoneDA;

        private IZoneDA ZoneDA
        {
            get
            {
                return this._zoneDA ?? (this._zoneDA = IocHelper.Container.Resolve<IZoneDA>());
            }
        }

        protected override void RegisterType()
        {
            IocHelper.Container.RegisterInstanceSingleton((IZoneDA)DAFactory.CreateDA(typeof(IZoneDA)));
        }
        #endregion

        public ZoneEntity RetrieveZoneById(long zoneId)
        {
            return this.ZoneDA.RetrieveZoneById(zoneId);
        }
        public ZoneEntity[] RetrieveAllZones()
        {
            return this.ZoneDA.RetrieveAllZones();
        }
    }
}