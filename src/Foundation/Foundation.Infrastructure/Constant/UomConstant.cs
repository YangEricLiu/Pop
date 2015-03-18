/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: UomConstant.cs
 * Author	    : Aries Zhang
 * Date Created : 2012-05-14
 * Description  : System constant UOM ids
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
namespace SE.DSP.Foundation.Infrastructure.Constant
{
    
    /// <summary>
    /// System constant UOM ids
    /// </summary>
    public class UomConstant
    {
        /// <summary>
        /// People count uom for hierarchy dynamic property
        /// </summary>
        public const long CONSTUOM_PEOPLECOUNT = 21;

        /// <summary>
        /// Area uom for hierarchy dynamic property
        /// </summary>
        public const long CONSTUOM_AREA = 13;

        /// <summary>
        /// Standard coal uom for carbon usage
        /// </summary>
        public const long CONSTUOM_STANDARDCOAL = 18;
        public const string CONSTUOM_STANDARDCOAL_CODE = "kgce";

        /// <summary>
        /// CO2 uom for carbon usage
        /// </summary>
        public const long CONSTUOM_CO2 = 19;
        public string CONSTUOM_CO2_CODE = "kgCO" + Convert.ToChar(0x2082);

        /// <summary>
        /// Tree uom for carbon usage
        /// </summary>
        public const long CONSTUOM_TREE = 20;
       // public const string CONSTUOM_TREE_CODE = "树";


        public static string CONSTUOM_TREE_CODE
        {
            get
            {
                return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Administration_CONSTUOM_TREE_CODE");
            }
        }

        /// <summary>
        /// Commodity that holds the constant UOMs
        /// </summary>
        public const long CONSTCOMMODITY_OTHER = 0;

        public const long CONSTCOMMODITY_Electricity = 1;

        public const long CONSTCOMMODITY_AIR = 12;

        public const long CONSTUOM_AIRQUALITY = 37;

        public const long CONSTUOM_KWH = 1;
        public const string CONSTUOM_KWH_CODE = "KWH";

        public const long CONSTUOM_T = 9;

        public string CONSTUOM_M2_CODE = "M" + Convert.ToChar(0x00b2);

        public const long CONSTUOM_M3 = 5;

        public const long CONSTUOM_RMB = 12;
        public const string CONSTUOM_RMB_CODE = "RMB";

        public const long CONSTUOM_PERSON = 21;
       // public const string CONSTUOM_PERSON_CODE = "人";

        public static string CONSTUOM_PERSON_CODE
        {
            get
            {
                return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Administration_CONSTUOM_PERSON_CODE");
            }
        }

        public const int COSTPRECISION = 2;
        public const int RATIOPRECISION = 2;
    }
}
