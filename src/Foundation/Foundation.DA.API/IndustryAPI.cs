using Microsoft.Practices.Unity;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.DA.Interface;


namespace SE.DSP.Foundation.DA.API
{
    public class IndustryAPI : DAAPIBase
    {
        #region DI
        private IIndustryDA _industryDA;

        private IIndustryDA IndustryDA
        {
            get
            {
                return this._industryDA ?? (this._industryDA = IocHelper.Container.Resolve<IIndustryDA>());
            }
        }

        protected override void RegisterType()
        {
            IocHelper.Container.RegisterInstanceSingleton((IIndustryDA)DAFactory.CreateDA(typeof(IIndustryDA)));
        }
        #endregion

        public IndustryEntity RetrieveIndustryById(long industryId)
        {
            return this.IndustryDA.RetrieveIndustryById(industryId);
        }
        public IndustryEntity[] RetrieveAllIndustries()
        {
            return this.IndustryDA.RetrieveAllIndustries();
        }
    }
}