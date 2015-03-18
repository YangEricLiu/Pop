using System;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.BE.Entities;

namespace SE.DSP.Foundation.DA.API
{
    public class UomAPI : DAAPIBase
    {
        #region DI
        private IUomDA _uomDA;
        private IUomDA UomDA
        {
            get
            {
                return this._uomDA ?? (this._uomDA = IocHelper.Container.Resolve<IUomDA>());
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

        private IUomGroupRelationDA _uomGroupRelationDA;
        private IUomGroupRelationDA UomGroupRelationDA
        {
            get
            {
                return this._uomGroupRelationDA ?? (this._uomGroupRelationDA = IocHelper.Container.Resolve<IUomGroupRelationDA>());
            }
        }

        protected override void RegisterType()
        {
            IocHelper.Container.RegisterInstanceSingleton((IUomDA)DAFactory.CreateDA(typeof(IUomDA)));
            IocHelper.Container.RegisterInstanceSingleton((IUomGroupDA)DAFactory.CreateDA(typeof(IUomGroupDA)));
            IocHelper.Container.RegisterInstanceSingleton((IUomGroupRelationDA)DAFactory.CreateDA(typeof(IUomGroupRelationDA)));
        }
        #endregion
        

        #region Uom
        public UomEntity RetrieveUomById(long uomId)
        {
            return this.UomDA.RetrieveUomById(uomId);
        }
        public UomEntity[] RetrieveAllUom()
        {
            return this.UomDA.RetrieveAllUom();
        }
        public UomEntity[] RetrieveUomByCommodityId(long commodityId)
        {
            return this.UomDA.RetrieveUomByCommodityId(commodityId);
        }
        #endregion

        #region Uom Group
        public UomGroupEntity RetrieveUomGroupById(long uomGroupId)
        {
            return this.UomGroupDA.RetrieveUomGroupById(uomGroupId);
        }
        public UomGroupEntity[] RetrieveUomGroupByCommodity(long commodityId)
        {
            return this.UomGroupDA.RetrieveUomGroupByCommodity(commodityId);
        }
        public UomGroupEntity RetrieveUomGroupByCommodityAndUom(long commodityId, long uomId)
        {
            return this.UomGroupDA.RetrieveUomGroupByCommodityAndUom(commodityId, uomId);
        }
        #endregion

        #region Uom Group Relation
        public UomGroupRelationEntity RetrieveCommodityUom(long commodityId, long uomId, long? spid=null)
        {
            return this.UomGroupRelationDA.RetrieveCommodityUom(commodityId, uomId);
        }
        public UomGroupRelationEntity[] RetrieveUomByGroup(long groupId)
        {
            return this.UomGroupRelationDA.RetrieveUomByGroup(groupId);
        }
        public UomGroupRelationEntity[] RetrieveCarbonConvertableCommodity()
        {
            return this.UomGroupRelationDA.RetrieveCarbonConvertableCommodity();
        }
        public UomGroupRelationEntity[] RetrieveUomRelationByCommodityId(long commodityId)
        {
            return this.UomGroupRelationDA.RetrieveUomRelationByCommodityId(commodityId);
        }
        public UomGroupRelationEntity[] RetrieveUomRelation()
        {
            return this.UomGroupRelationDA.RetrieveUomRelation();
        }
        #endregion
    }
}