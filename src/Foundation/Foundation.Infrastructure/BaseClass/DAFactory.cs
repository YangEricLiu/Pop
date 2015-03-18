/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: DAFactory.cs
 * Author	    : Figo
 * Date Created : 2011-09-28
 * Description  : Factory for data access
--------------------------------------------------------------------------------------------*/

using System;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting;

namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    /// <summary>
    /// Factory for data access
    /// </summary>
    public static class DAFactory
    {
        /// <summary>
        /// Create data access instance for the data access contract.
        /// </summary>
        /// <param name="daContractType">Data access contract type.</param>
        /// <returns>Data access instance.</returns>
        public static Object CreateDA(Type daContractType)
        {
            var typename = daContractType.FullName;

            if (typename.IndexOf(".Interface.") > -1)
            {
                const string WILDCARD = "*";
                string daAssemblySuffix = "Service";

                if (ConfigurationManager.AppSettings.AllKeys.Contains(daContractType.Name))
                {
                    daAssemblySuffix = ConfigurationManager.AppSettings[daContractType.Name];
                }
                else if (ConfigurationManager.AppSettings.AllKeys.Contains(WILDCARD))
                {
                    //daAssemblySuffix = ConfigurationManager.AppSettings[WILDCARD];
                }

                const string DACONTRACTASSEMBLYNAMESUFFIX = "Interface";

                string contractAssemblyName = daContractType.Assembly.GetName().Name;
                string contractTypeNamespace = daContractType.Namespace;
                string contractTypeName = daContractType.Name;

                string daAssemblyName = contractAssemblyName.Replace(DACONTRACTASSEMBLYNAMESUFFIX, daAssemblySuffix);
                string daTypeName = daContractType.Namespace.Replace(DACONTRACTASSEMBLYNAMESUFFIX, daAssemblySuffix) + "." + contractTypeName.Remove(0, 1);

                ObjectHandle da = Activator.CreateInstance(daAssemblyName, daTypeName);

                return da.Unwrap();
            }
            else
            {
                const string WILDCARD = "*";
                string daAssemblySuffix = "MSSQL";

                if (ConfigurationManager.AppSettings.AllKeys.Contains(daContractType.Name))
                {
                    daAssemblySuffix = ConfigurationManager.AppSettings[daContractType.Name];
                }
                else if (ConfigurationManager.AppSettings.AllKeys.Contains(WILDCARD))
                {
                    //daAssemblySuffix = ConfigurationManager.AppSettings[WILDCARD];
                }

                const string DACONTRACTASSEMBLYNAMESUFFIX = "Contract";

                string contractAssemblyName = daContractType.Assembly.GetName().Name;
                string contractTypeNamespace = daContractType.Namespace;
                string contractTypeName = daContractType.Name;

                string daAssemblyName = contractAssemblyName.Replace(DACONTRACTASSEMBLYNAMESUFFIX, daAssemblySuffix);
                string daTypeName = daContractType.Namespace.Replace(DACONTRACTASSEMBLYNAMESUFFIX, daAssemblySuffix) + "." + contractTypeName.Remove(0, 1);

                ObjectHandle da = Activator.CreateInstance(daAssemblyName, daTypeName);

                return da.Unwrap();
                
            }
        }
    }
}