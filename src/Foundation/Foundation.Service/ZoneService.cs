using Microsoft.Practices.Unity;
using SE.DSP.Foundation.API;
using SE.DSP.Foundation.DA.API;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Utils;
using System.Linq;

namespace SE.DSP.Foundation.Service
{
    public class ZoneService : ServiceBase, IZoneService
    {
        #region DI
        private ZoneAPI _zoneAPI;

        private ZoneAPI ZoneAPI
        {
            get
            {
                return this._zoneAPI ?? (this._zoneAPI = IocHelper.Container.Resolve<ZoneAPI>());
            }
        }

        protected override void RegisterType()
        {
            IocHelper.Container.RegisterType<ZoneAPI, ZoneAPI>(new ContainerControlledLifetimeManager());
        }
        #endregion

        /// <summary>
        /// Get the specified zone by its id
        /// </summary>
        /// <param name="ZoneId">id of the desired zone</param>
        /// <returns></returns>
        public ZoneDto GetZoneById(long zoneId)
        {
            var entity = this.ZoneAPI.RetrieveZoneById(zoneId);

            return ZoneTranslator.Entity2Dto(entity);
        }

        /// <summary>
        /// Get all zones defined in the system
        /// </summary>
        /// <returns></returns>
        public ZoneDto[] GetAllZones(bool includeRoot)
        {
            var entities = this.ZoneAPI.RetrieveAllZones();

            if (!includeRoot)
            {
                entities = entities.Where(entity => entity.ParentId.HasValue).ToArray();
            }

            return ZoneTranslator.Entities2Dtos(entities);
        }
    }
}