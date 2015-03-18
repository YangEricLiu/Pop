/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: Resource.cs
 * Author	    : Mike
 * Date Created : 2014-5-26
 * Description  : Helper for Getting Resource
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/


using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class I18nHelper
    {
        static NameValueCollection deployConfig =
            (NameValueCollection)ConfigurationManager.GetSection("extjslocaleConfiguration");

        static Dictionary<String, Dictionary<String, String>> localeStringDict = null;
        static object _locker = new object();

        public static String GetExtjsLocaleName(Language language)
        {
            if (deployConfig == null) return null;
            return deployConfig.Get(LocaleEnumToString(language));
        }

        /// <summary>
        /// key:resource file Path,value resource file dictionary
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> fileDict = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// get resouce file key's value. If it doesn't get value,will return the key.
        /// </summary>
        /// <param name="language">current language</param>
        /// <param name="resourceType">app or db</param>
        /// <param name="key">resource key</param>
        /// <returns>return the key's value of the resource file </returns>
        public static string GetValue(Language language, I18nResourceType resourceType, string key)
        {
            string resPath = GetResourceFilePath(language, resourceType);

            if (string.IsNullOrEmpty(resPath))
            {
                LogHelper.LogError("params error! language or resourceType error!");
                return key;
            }

            int res = 0;

            lock (_locker)
            {
                res = LoadResourceFile(resPath);
            }

            if (res != 0)
            {
                LogHelper.LogError(string.Format("Load file [{0}] failed", resPath));
                return key;
            }

            if (fileDict.ContainsKey(resPath))
            {
                if (fileDict[resPath].ContainsKey(key))
                {
                    return fileDict[resPath][key];
                }
                else
                {
                    LogHelper.LogError(string.Format("file [{0}] doesn't find key=[{1}]", resPath, key));
                    return key;
                }
            }

            return null;



        }

        /// <summary>
        /// get multiply keys' values
        /// </summary>
        /// <param name="language">language</param>
        /// <param name="resourceType">App or DB</param>
        /// <param name="keys">keys array</param>
        /// <returns>return key-value pairs(key,value)</returns>
        public static Dictionary<string, string> GetValues(Language language, I18nResourceType resourceType, string[] keys)
        {
            if (keys == null || keys.Length == 0) return null;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var temp in keys)
            {
                string value = GetValue(language, resourceType, temp);
                if (!dict.ContainsKey(temp))
                {
                    dict.Add(temp, value);
                }
            }

            return dict;
        }

        private static string GetResourceFilePath(Language language, I18nResourceType resourceType)
        {
            switch (language)
            {
                case Language.ZH_CN:
                    return String.Format("SE.DSP.Foundation.Infrastructure.Resource.zh_CN.{0}.resources", resourceType);
                case Language.EN_US:
                    return String.Format("SE.DSP.Foundation.Infrastructure.Resource.en_US.{0}.resources", resourceType);
            }
            return null;
        }

        private static int LoadResourceFile(string fileName)
        {
            //if (string.IsNullOrEmpty(fileName) ||
            //     !File.Exists(fileName))
            //    return -1;

            //FileInfo fi = new FileInfo(fileName);
            //if (!fi.Extension.Trim().Contains("resx"))
            //    return -1;

            if (fileDict.ContainsKey(fileName))
                return 0;

         //   fileName = fileName.Replace('-', '_');
            Dictionary<string, string> dict = new Dictionary<string, string>();


            Assembly assembly = typeof(I18nHelper).Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(fileName))
            {
                using (ResourceReader rs = new ResourceReader(stream))
                {
                    var cursor = rs.GetEnumerator();
                    while (cursor.MoveNext())
                    {
                        if (!dict.ContainsKey(cursor.Key.ToString()))
                        {
                            dict.Add(cursor.Key.ToString(), cursor.Value.ToString().Trim());
                        }
                    }
                }
            }

            if (!fileDict.ContainsKey(fileName))
            {
                fileDict.Add(fileName, dict);
            }

            return 0;

        }

        public static Language LocaleStringToEnum(String locale)
        {
            var lang = default(Language);

            if (String.IsNullOrEmpty(locale)) return lang;

            var langEnumStr = locale.Replace('-', '_');

            System.Enum.TryParse<Language>(langEnumStr, true, out lang);

            return lang;
        }

        public static String LocaleEnumToString(Language language)
        {
            switch (language)
            {
                case Language.ZH_CN:
                    return "zh-CN";
                case Language.EN_US:
                    return "en-US";
                default:
                    break;
            }
            return String.Empty;
        }
    }
}
