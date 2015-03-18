using System;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.BaseClass;

namespace SE.DSP.Foundation.DA.API
{
    public class CommodityAPI : DAAPIBase
    {
        #region DI
        private ICommodityDA _commodityDA;
        private ICommodityDA CommodityDA
        {
            get
            {
                return this._commodityDA ?? (this._commodityDA = IocHelper.Container.Resolve<ICommodityDA>());
            }
        }


        protected override void RegisterType()
        {
            IocHelper.Container.RegisterInstanceSingleton((ICommodityDA)DAFactory.CreateDA(typeof(ICommodityDA)));
        }
        #endregion
        
        #region Commodity
        public CommodityEntity RetrieveCommodityById(long commodityId)
        {
            return this.CommodityDA.RetrieveCommodityById(commodityId);
        }
        public CommodityEntity[] RetrieveAllCommodity()
        {
            return this.CommodityDA.RetrieveAllCommodity();
        }
        #endregion

    }
}