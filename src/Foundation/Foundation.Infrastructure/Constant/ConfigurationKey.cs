/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: ConfigurationKey.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Configuration key in the config files
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Constant
{
    /// <summary>
    /// The keys of common app setting.
    /// </summary>
    public static class ConfigurationKey
    {
        /// <summary>
        /// The key of ServerID app setting.
        /// </summary>
        public const string SERVERID = "ServerId";

        /// <summary>
        /// The key of LoggingSeverity app setting.
        /// </summary>
        public const string LOGGINGSEVERITY = "LoggingSeverity";
        

        public const string EXPORTPHANTOMPATH = "phantomPath";
        public const string EXPORTCONVERTJSPATH = "convertJsPath";
        public const string EXPORTTEMPFOLDER = "tempFolder";
        public const string DEMOTEMPLATE = "DemoTemplate";
    }
}
