using Microsoft.Practices.Unity;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Web.Wcf;

namespace SE.DSP.Pop.BL.AppHost.Common.Startup
{
    public class IocConfiguration : IGlobalConfiguration
    {
        public void Configure()
        {
            IocHelper.Container.RegisterInstance(typeof(SE.DSP.Foundation.API.IUserService), ServiceProxy<SE.DSP.Foundation.API.IUserService>.GetClient("IUserService.EndPoint"));
            IocHelper.Container.RegisterInstance(typeof(SE.DSP.Foundation.API.IZoneService), ServiceProxy<SE.DSP.Foundation.API.IZoneService>.GetClient("IZoneService.EndPoint"));
            IocHelper.Container.RegisterInstance(typeof(SE.DSP.Foundation.API.IUomService), ServiceProxy<SE.DSP.Foundation.API.IUomService>.GetClient("IUomService.EndPoint"));
            IocHelper.Container.RegisterInstance(typeof(SE.DSP.Foundation.API.IServiceProviderService), ServiceProxy<SE.DSP.Foundation.API.IServiceProviderService>.GetClient("IServiceProviderService.EndPoint"));
            IocHelper.Container.RegisterInstance(typeof(SE.DSP.Foundation.API.IIndustryService), ServiceProxy<SE.DSP.Foundation.API.IIndustryService>.GetClient("IIndustryService.EndPoint"));
            IocHelper.Container.RegisterInstance(typeof(SE.DSP.Foundation.API.ICommodityService), ServiceProxy<SE.DSP.Foundation.API.ICommodityService>.GetClient("ICommodityService.EndPoint"));
            IocHelper.Container.RegisterInstance(typeof(SE.DSP.Foundation.API.IAccessControlService), ServiceProxy<SE.DSP.Foundation.API.IAccessControlService>.GetClient("IAccessControlService.EndPoint"));
        }
    }
}