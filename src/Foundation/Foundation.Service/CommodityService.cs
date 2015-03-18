using Microsoft.Practices.Unity;

using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.API;
using SE.DSP.Foundation.Infrastructure.Constant;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.DA.Interface;

namespace SE.DSP.Foundation.Service
{
    public class CommodityService : ServiceBase, ICommodityService
    {
        private CacheHelper CommodityCacheHelper = new CacheHelper("commodity", ServiceContext.CurrentUser.SPId);

        #region DI
        private IUomDA _uomAPI;
        private IUomDA UomAPI
        {
            get
            {
                return this._uomAPI ?? (this._uomAPI = IocHelper.Container.Resolve<IUomDA>());
            }
        }

        private IUomGroupRelationDA _uomGroupRelationDA;
        private IUomGroupRelationDA UomGroupRelationDA
        {
            get
            {
                return this._uomGroupRelationDA ?? (this._uomGroupRelationDA = IocHelper.Container.Resolve<IUomGroupRelationDA>());
            }
        }

        private ICommodityDA _CommodityAPI;
        private ICommodityDA CommodityAPI
        {
            get
            {
                return this._CommodityAPI ?? (this._CommodityAPI = IocHelper.Container.Resolve<ICommodityDA>());
            }
        }
 
        #endregion

        /// <summary>
        /// Get the specified commodity by its id
        /// </summary>
        /// <param name="commodityId">id of the desired commodity</param>
        /// <returns></returns>
        public CommodityDto GetCommodityById(long commodityId)
        {
            return CommodityCacheHelper.Get<long, CommodityDto>(commodityId, () =>
            {
                var commodityEntity = this.CommodityAPI.RetrieveCommodityById(commodityId);

                return CommodityTranslator.CommodityEntity2CommodityDto(commodityEntity);
            });
        }

        /// <summary>
        /// Get all commodities defined in the system
        /// </summary>
        /// <param name="returnsOther">specifies whether the "Other" commodity will be included in the return value</param>
        /// <returns></returns>
        public CommodityDto[] GetAllCommodity(bool? returnsOther)
        {
            List<CommodityDto> commodityDtos = new List<CommodityDto>();
            var entities = this.CommodityAPI.RetrieveAllCommodity();

            if (returnsOther.HasValue)
            {
                if (returnsOther.Value)
                    entities = entities.Where(c => c.Id.Value == UomConstant.CONSTCOMMODITY_OTHER).ToArray();
                else
                    entities = entities.Where(c => c.Id.Value != UomConstant.CONSTCOMMODITY_OTHER).ToArray();
            }

            var query = from e in entities select CommodityTranslator.CommodityEntity2CommodityDto(e, this.UomGroupRelationDA.RetrieveUomRelationByCommodityId(e.Id.Value));

            return query.ToArray();
        }
    }
}