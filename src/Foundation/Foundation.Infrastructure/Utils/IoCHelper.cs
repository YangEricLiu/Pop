/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: IocHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Helper for Ioc
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public static class IUnitContainerREMExtensions
    {
        public static void RegisterInstanceSingleton<TInterface>(this IUnityContainer container, TInterface obj)
        {
            //if (!IocHelper.Container.IsRegistered<TInterface>()) //this is not thread safe
            //{
            IocHelper.Container.RegisterInstance(obj);
            //}
        }
    }
    /// <summary>
    /// IoC utility class.
    /// </summary>
    public static class IocHelper
    {
        private static readonly IUnityContainer c;

        static IocHelper()
        {
            c = new UnityContainer();
            var unitySection = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            if (unitySection != null)
            {
                c.LoadConfiguration(unitySection);
            }
        }

        /// <summary>
        /// A sigleton <see cref="UnityContainer" /> object.
        /// </summary>
        /// <remarks>All dependences of app service api, bl and da api objects should be injected from this container.</remarks>
        public static IUnityContainer Container
        {
            get
            {
                return c;
            }
        }
    }
}