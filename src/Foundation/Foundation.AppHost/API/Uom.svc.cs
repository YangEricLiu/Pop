using SE.DSP.Foundation.API;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Practices.Unity;

namespace SE.DSP.Foundation.AppHost.API
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Uom : ServiceBase, IUomService
    {
        private CacheHelper UOMCacheHelper = new CacheHelper("uom", SE.DSP.Foundation.Infrastructure.Interception.ServiceContext.CurrentUser.SPId);

        #region DI
        private IUomDA _uomAPI;
        private IUomDA UomAPI
        {
            get
            {
                return this._uomAPI ?? (this._uomAPI = IocHelper.Container.Resolve<IUomDA>());
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

        private IUomGroupRelationDA _uomGroupRelationDA;
        private IUomGroupRelationDA UomGroupRelationDA
        {
            get
            {
                return this._uomGroupRelationDA ?? (this._uomGroupRelationDA = IocHelper.Container.Resolve<IUomGroupRelationDA>());
            }
        }

        private IUomGroupDA _uomGroupDA;
        private IUomGroupDA UomGroupDA
        {
            get
            {
                return this._uomGroupDA ?? (this._uomGroupDA = IocHelper.Container.Resolve<IUomGroupDA>());
            }
        }

        #endregion

        #region Uom
        /// <summary>
        /// Get the specified uom by its id 
        /// </summary>
        /// <param name="uomId">id of the desired uom</param>
        /// <returns></returns>
        public UomDto GetUomById(long uomId)
        {
            return UOMCacheHelper.Get<long, UomDto>(uomId, () =>
            {
                var uomEntity = this.UomAPI.RetrieveUomById(uomId);
                return UomTranslator.UomEntity2UomDto(uomEntity);
            });
        }

        /// <summary>
        /// Get an array of uoms related to the specified commodity
        /// </summary>
        /// <param name="commodityId">id of the specified commodity</param>
        /// <returns></returns>
        public UomDto[] GetUomByCommodityId(long commodityId)
        {
            var uoms = this.UomAPI.RetrieveUomByCommodityId(commodityId);
            return UomTranslator.UomEntities2UomDtoes(uoms);
        }

        /// <summary>
        /// Get all uoms defined in the system
        /// </summary>
        /// <returns></returns>
        public UomDto[] GetAllUom()
        {
            var uoms = this.UomAPI.RetrieveAllUom();
            return UomTranslator.UomEntities2UomDtoes(uoms);
        }

        /// <summary>
        /// Get id of the common uom of a uom group specified by commodity id and uom id
        /// </summary>
        /// <param name="commodityId">id of the specified commodity</param>
        /// <param name="uomId">id of the specified uom</param>
        /// <returns>id of the common uom in the uom group, if there is no common uom in this group or the specified commodity and uom do not match, return 0</returns>
        public long GetCommonUomId(long commodityId, long uomId)
        {
            UomGroupRelationEntity[] Uoms = this.RetrieveUomInSameGroup(commodityId, uomId);
            UomGroupRelationEntity[] query = Uoms.Where(u => u.IsCommon).ToArray();
            return query.Length == 1 ? query[0].UomId : 0;
        }

        public UomGroupRelationEntity GetCommonUom(long commodityId, long uomId)
        {
            UomGroupRelationEntity[] Uoms = this.RetrieveUomInSameGroup(commodityId, uomId);
            UomGroupRelationEntity[] query = Uoms.Where(u => u.IsCommon).ToArray();
            return query.Length == 1 ? query[0] : null;
        }

        public UomGroupRelationEntity GetCommonUomInGroup(long groupId)
        {
            var uoms = this.UomGroupRelationDA.RetrieveUomByGroup(groupId);

            return uoms.Where(uom => uom.IsCommon).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commodityId"></param>
        /// <param name="uomId"></param>
        /// <returns></returns>
        public UomGroupEntity GetUomGroupByCommodityAndUom(long commodityId, long uomId)
        {
            return this.UomGroupDA.RetrieveUomGroupByCommodityAndUom(commodityId, uomId);
        }

        public UomGroupEntity GetEnergyConsumptionGroupByCommodity(long commodityId)
        {
            var groups = this.UomGroupDA.RetrieveUomGroupByCommodity(commodityId);

            return groups.Where(group => group.IsEnergyConsumption).FirstOrDefault();
        }


        /// <summary>
        /// Get an uom group relation entity by the specified commodity and uom, 
        /// this relation entity contains additional commodity code and uom code field
        /// </summary>
        /// <param name="commodityId">id of the specified commodity</param>
        /// <param name="uomId">id of the specified uom</param>
        /// <returns></returns>
        public UomGroupRelationEntity GetCommodityUom(long commodityId, long uomId)
        {

            return this.UomGroupRelationDA.RetrieveCommodityUom(commodityId, uomId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public UomGroupRelationEntity[] GetUomByGroup(long groupId)
        {
            return UomGroupRelationDA.RetrieveUomByGroup(groupId);
        }
        #endregion

        #region Uom conversion framework

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UomConversionDto[] GetCommonConversionFactors()
        {
            List<UomConversionDto> result = new List<UomConversionDto>();

            //get all commodity uom relations
            foreach (UomGroupRelationEntity relation in this.UomGroupRelationDA.RetrieveUomRelation())
            {
                UomConversionDto conversion = new UomConversionDto();

                conversion.CommodityId = relation.Commodity.Id.Value;
                conversion.SourceUomId = relation.UomId;
                conversion.DestinationUomId = relation.IsCommon ? relation.UomId : this.GetCommonUomId(conversion.CommodityId, conversion.SourceUomId);
                conversion.ConversionFactor = this.GetUomFactorToCommonUom(conversion.CommodityId, conversion.SourceUomId);

                result.Add(conversion);
            }

            return result.ToArray();
        }


        /// <summary>
        /// Get uom factor from the specified source uom to destination uom
        /// </summary>
        /// <param name="commodityId"></param>
        /// <param name="sourceUomId"></param>
        /// <param name="destinationUomId"></param>
        /// <returns>Nullable factor</returns>
        public decimal? GetUomFactorToDestinationUom(long commodityId, long sourceUomId, long destinationUomId)
        {
            UomGroupRelationEntity sourceUomEntity = GetCommodityUom(commodityId, sourceUomId);
            if (sourceUomEntity == null)
            {
                return null;
            }

            UomGroupRelationEntity[] GroupUom = this.RetrieveUomInSameGroup(commodityId, sourceUomId).Where(r => r.UomId == destinationUomId).ToArray();
            if (GroupUom == null || GroupUom.Length != 1)
            {
                return null;
            }

            UomGroupRelationEntity destinationUomEntity = GroupUom[0];

            if (sourceUomEntity.UomId == destinationUomEntity.UomId)
            {
                return 1M;
            }

            return sourceUomEntity.Factor / destinationUomEntity.Factor;
        }

        #region private methods


        /// <summary>
        /// Get the conversion factor from the specified uom to common uom of the uom group
        /// </summary>
        /// <param name="commodityId">id of the specified commodity</param>
        /// <param name="uomId">id of the specified uom</param>
        /// <returns></returns>
        private decimal? GetUomFactorToCommonUom(long commodityId, long uomId)
        {
            // factor * 1 base = 1 uom (1000 kg = 1 t), 
            //if the input uom is common, return 1
            UomGroupRelationEntity relationEntity = GetCommodityUom(commodityId, uomId);
            if (relationEntity == null)
            {
                return null;
            }

            if (relationEntity.IsCommon)
            {
                return 1M;
            }

            //if the input uom is base, return 1/factor (1 base = 1/factor common)
            //if the input uom is not base, (1 base = 1/factor1 common = 1/factor2 uom, 1 uom = factor2/factor1 common)
            UomGroupRelationEntity[] GroupUom = this.RetrieveUomInSameGroup(commodityId, uomId).Where(r => r.IsCommon == true).ToArray();
            if (GroupUom != null && GroupUom.Length == 1)
            {
                UomGroupRelationEntity CommonUom = GroupUom[0];
                return relationEntity.Factor / CommonUom.Factor;
            }

            return null;
        }

        private decimal? GetUomFactorToBaseUom(long commodityId, long uomId)
        {
            //any factor to base uom is defined in their factor field
            UomGroupRelationEntity relationEntity = GetCommodityUom(commodityId, uomId);

            if (relationEntity == null)
            {
                return null;
            }

            return relationEntity.Factor;
        }

        private decimal? GetUomFactorToStandardCoalUom(long commodityId, long uomId)
        {
            UomGroupRelationEntity relationEntity = GetCommodityUom(commodityId, uomId);

            if (relationEntity == null)
            {
                return null;
            }

            if (relationEntity.IsStandardCoal)
            {
                return 1M;
            }

            UomGroupRelationEntity[] GroupUom = this.RetrieveUomInSameGroup(commodityId, uomId).Where(r => r.IsStandardCoal == true).ToArray();
            if (GroupUom != null && GroupUom.Length == 1)
            {
                UomGroupRelationEntity StandardCoalUom = GroupUom[0];

                return relationEntity.Factor / StandardCoalUom.Factor;
            }

            return null;
        }

        /// <summary>
        /// Get the carbon conversion factor of the specified year from an array of carbon factor
        /// </summary>
        /// <param name="carbonFactors">The array of carbon factor to be searched</param>
        /// <param name="year">The specified year</param>
        /// <returns>The carbon conversion factor of the specified year, null if not found</returns>
        //private decimal? GetCarbonFactorByYear(CarbonFactorItemDto[] carbonFactors, int year)
        //{
        //    //the factors array is already ordered by year desc
        //    //find the first factor which its effective year is less than or equal to the specified year
        //    var usfulFactors = carbonFactors.OrderByDescending(f => f.EffectiveYear).Where(f => f.EffectiveYear <= year).ToArray();
        //    return usfulFactors.Length > 0 ? (decimal?)usfulFactors.FirstOrDefault().FactorValue : null;
        //}
        #endregion
        #endregion

        #region Common
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commodityId"></param>
        /// <param name="uomId"></param>
        /// <returns></returns>
        private UomGroupRelationEntity[] RetrieveUomInSameGroup(long commodityId, long uomId)
        {
            var group = this.UomGroupDA.RetrieveUomGroupByCommodityAndUom(commodityId, uomId);
            return group == null ? new UomGroupRelationEntity[0] : this.UomGroupRelationDA.RetrieveUomByGroup(group.Id.Value);
        }
        #endregion
    }
}
