/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: ConstantValue.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Constant values
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
namespace SE.DSP.Foundation.Infrastructure.Constant
{
    /// <summary>
    /// The common constants.
    /// </summary>
    public static class ConstantValue
    {
        /// <summary>
        /// Separator between items
        /// </summary>
        public const string Slash = "/";

        public const string BackSlash = @"\";

        public const char Comma = ',';

        public const char Delimiter = '|';

        /// <summary>
        /// The length limitation constant of entity code.
        /// </summary>
        public const int CODELENGTHLIMITATION = 100;

        /// <summary>
        /// The regex of entity code
        /// </summary>
        ///  @"^[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\#\&\,\;\.\~\+\%:\\\|/]+( +[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\#\&\,\;\.\~\+\%:\\\|/]+)*$";
        public static string CODEREGEX = ConfigHelper.Get(DeploymentConfigKey.CODEREGEX);

        /// <summary>
        /// The regex of entity code
        /// </summary>
        /// @"^[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\{\}\#\&\,\;\.\~\+\%:]+( +[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\{\}\#\&\,\;\.\~\+\%:]+)*$";
        public static string CUSTOMERCODEREGEX = ConfigHelper.Get(DeploymentConfigKey.CUSTOMERCODEREGEX);

        /// <summary>
        /// The regex of entity code
        /// </summary>
        /// @"^[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\#\&\,\;\.\~\+\%:\\\|/]+( +[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\#\&\,\;\.\~\+\%:\\\|/]+)*$"
        public static string METERCODEREGEX = ConfigHelper.Get(DeploymentConfigKey.METERCODEREGEX);

        /// <summary>
        /// The regex of entity code(all printable ascii charactor).
        /// </summary>
        //public const string SPECIALCODEREGEX = @"^[\x20-\x7e]+$";

        /// <summary>
        /// The length limitation constant of entity name.
        /// </summary>
        public const int NAMELENGTHLIMITATION = 100;

        /// <summary>
        /// The regex of entity name(Only Chinese Character, a-z, A-Z, 0-9, ‘_’ and ‘ ‘ are supported. ).
        /// </summary>
        public const string NAMEREGEX = @"^[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\{\}\#\&\,\;\.\~\+\%:]+( +[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\{\}\#\&\,\;\.\~\+\%:]+)*$";
        /// <summary>
        /// The length limitation constant of entity comment.
        /// </summary>
        public const int COMMENTLENGTHLIMITATION = int.MaxValue;

        /// <summary>
        /// The regex of entity comment(0-9, a-z, A-Z, _, Chinese chars).
        /// </summary>
        public const string COMMENTREGEX = @"[\w\W\u4e00-\u9fa5]*$";

        /// <summary>
        /// The length limitation constant of password.
        /// </summary>
        public const int PASSWORDLENGTHMINLIMITATION = 6;
        public const int PASSWORDLENGTHMAXLIMITATION = 20;

        public const string PasswordRule1 = @"[0-9]+";
        public const string PasswordRule2 = @"[a-zA-Z]+";
        public const string PasswordRule3 = @"^[0-9a-zA-Z_!@#$%^&*()][0-9a-zA-Z_!@#$%^&*()]*$";

        //TODO: timezone
        public const decimal TimeOffSet = 8m;

        /// <summary>
        /// lower year bound of jazz
        /// </summary>
        public const int EFFECTIVELOWERYEARBOUND = 1950;

        /// <summary>
        /// upper year bound of jazz
        /// </summary>
        public const int EFFECTIVEUPPERYEARBOUND = 2050;

        public const int EMAILMAXLENGTH = 100;
        public const string EMAILREGEX = @"^(\w)+((-\w+)*(\.\w+)*)*((\.\w+)*(-\w+)*)*@(\w(-\w)*(\.\w)*)+((\.\w+)+)$";

        public const int TELEPHONEMAXLENGTH = 100;
        public const string TELEPHONEREGEX = @"^(\d)+(-(\d)+)*$";

        public const int CUSTOMERNAMELENGTHLIMITATION = 100;
        public const string CUSTOMERNAMEREGEX = @"^[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\{\}\#\&\,\;\.\~\+\%:]+( +[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\{\}\#\&\,\;\.\~\+\%:]+)*$";

        public const int PERSONNAMELENGTHLIMITATION = 100;
        public const string PERSONNAMEREGEX = @"^[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\{\}\#\&\,\;\.\~\+\%:]+( +[\u4e00-\u9fa50-9a-zA-Z_\(\)\-\[\]\{\}\#\&\,\;\.\~\+\%:]+)*$";
                
        public const int ADDRESSMAXLENGTH = 100;
        public const string ADDRESSREGEX = @"[\w\W\u4e00-\u9fa5]+$";

        public const string FEEDBACKPROBLEMDESCRIPTIONREGEX = @"[\w\W\u4e00-\u9fa5]+$";
        public const int FEEDBACKPROBLEMDESCRIPTIONMAXLENGTH = int.MaxValue;

        public const int USERIDMAXLENGTH = 100;
        public const string USERIDREGEX = @"^[\u4e00-\u9fa5a-zA-Z0-9_\-\.]+( +[\u4e00-\u9fa5a-zA-Z0-9_\-\.])*$";

        public const double INTMAX = 999999999999999;
        public const double INTMIN0 = 0;
        public const double INTMIN1 = 1;

        public const double DOUBLEMAX = 999999999.999999;
        public const double DOUBLEMIN = -999999999.999999;
        public const double DOUBLEMINPositive = 0.000001;

        public const int PLATFORMADMINSPID = -1;

        public const string YYYYMMDDHHMM = "yyyy-MM-dd HH:mm";


        public const long CommodityElectricityId = 1;

        public const string DemoUserName = "Demo account";
    }
}
