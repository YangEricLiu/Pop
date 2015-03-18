namespace SE.DSP.Foundation.Web
{
 
    using SE.DSP.Foundation.Infrastructure.Enumerations;
    using SE.DSP.Foundation.Infrastructure.Interception;
    using SE.DSP.Foundation.Infrastructure.Utils;
    

    public static class Constant
    {
        //public static string YEAR = "年";
        //public static string MONTH = "月";
        //public static string DAY = "日";
        //public static string HOUR = "点";

        //public static string TIMEHEADERSTRING = "时间";
        //public static string VALUEHEADERSTRING = "值";
        //public static string ENERGYEXPORTTITLE = "能效分析";
        //public static string COSTEXPORTTITLE = "成本";
        //public static string CARBONEXPORTTITLE = "碳排放";
        //public static string KPIEXPORTTITLE = "关键能效指标";
        //public static string KPIBASELINEHEADER = "基准值";
        //public static string KPITARGETLINEHEADER = "基准值";

        private static string GetValue(string key)
        {
            return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, key);
        }

        public const string ERRORPARAMETERNAME = "error";

        public static string YEAR
        {
            get
            {
                return GetValue("WEBCOMMONCONST_YEAR");
            }
        }

        public static string MONTH
        {
            get
            {
                return GetValue("WEBCOMMONCONST_MONTH");
            }
        }

        public static string DAY
        {
            get
            {
                return GetValue("WEBCOMMONCONST_DAY");
            }
        }
       
        public static string HOUR
        {
            get
            {
                return GetValue("WEBCOMMONCONST_HOUR");
            }
        }

        public static string TIMEHEADERSTRING
        {
            get
            {
                return GetValue("ENERGYCONST_TIMEHEADERSTRING");
            }
        }


        public static string VALUEHEADERSTRING
        {
            get
            {
                return GetValue("ENERGYCONST_TIMEHEADERSTRING");
            }
        }

        public static string ENERGYEXPORTTITLE
        {
            get
            {
                return GetValue("ENERGYCONST_ENERGYEXPORTTITLE");
            }
        }

        public static string COSTEXPORTTITLE
        {
            get
            {
                return GetValue("ENERGYCONST_COSTEXPORTTITLE");
            }
        }

        public static string CARBONEXPORTTITLE
        {
            get
            {
                return GetValue("ENERGYCONST_CARBONEXPORTTITLE");
            }
        
        }

        public static string KPIEXPORTTITLE
        {
            get
            {
                return GetValue("ENERGYCONST_KPIEXPORTTITLE");
            }
        }

        public static string KPIBASELINEHEADER
        {
            get
            {
                return GetValue("ENERGYCONST_KPIBASELINEHEADER");
            }
        }

        public static string KPITARGETLINEHEADER
        {
            get
            {
                return GetValue("ENERGYCONST_KPITARGETLINEHEADER");
            }
        }

        public static string SESSIONNAME = "User";
        public static string LASTLOGINNAME = "Username";
        public static string LANGUAGEQUERYSTRING = "lang";
        public static string PLACEHOLDERURL = "http://placeholder/";




    
    }
}