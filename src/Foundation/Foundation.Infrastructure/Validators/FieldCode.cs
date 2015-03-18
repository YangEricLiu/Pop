using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Foundation.Infrastructure.Validators
{

    //Jacob: Please ensure the field string length as two numbers;
    //Validation code range is 900-999
    public static class FieldCode
    {

        //for BaseDto 
        public const string NAME = "90";
        public const string ID = "91";
        public const string VERSION = "92";

        public const string CODE = "93";
        public const string METERCODE = "94";
        public const string COMMENT = "95";

        //For your special code, please use 9XY(X>5, Y>=0), eg.960,990
        //for carbon factor
        public const string EffectiveYear = "960";
        public const string ChannelId = "961";
        public const string UomId = "962";
        public const string CommodityId = "963";
        public const string HierarchyId = "964";
        public const string AreaDimensionId = "965";
        public const string SystemDimensionId = "966";
        public const string TagId = "967";
        public const string CustomerId = "969";
        public const string Formula = "970";
        public const string TBScope = "971";
        public const string TBId = "972";
        public const string NormalDates = "973";
        public const string SpecialDates = "974";
        public const string Year = "975";
        public const string Month = "976";
        public const string StartTime = "977";
        public const string ParentId = "978";
        public const string UserId = "979";
        public const string TemplateItemId = "980";//special for systemdimension
        public const string Password = "981";
        public const string Email = "982";
        public const string DashboardId = "983";
        public const string SendShareInfoId = "984";
        public const string WidgetId = "985";


    }

    //Jacob: Please ensure the field string length only one number;
    public class ErrorCode
    {
        public const string RANGEERROR = "0";   //for int value range errors
        public const string NULLERROR = "1";
        public const string LENGTHERROR = "2";
        public const string FORMATERROR = "3";

    }
}