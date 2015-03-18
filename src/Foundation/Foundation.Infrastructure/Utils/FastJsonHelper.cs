/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: FastJsonHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Helper for FastJson
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public class FastJsonHelper
    {
        private static fastJSON.JSONParamters _jsonParams = new fastJSON.JSONParamters() { UseExtensions = true, UsingGlobalTypes = true, UseUTCDateTime = false,};
        private static fastJSON.JSON _json = fastJSON.JSON.Instance;
        
        public static string Object2String(Object obj)
        {
            return _json.ToJSON(obj, _jsonParams);
        }

        public static T String2Object<T>(string str)
        {
            _json.Param = _jsonParams;
            return _json.ToObject<T>(str);
        }

        public static object String2Object(string str)
        {
            _json.Param = _jsonParams;
            return _json.ToObject(str);
        }

        public static string Beautify(string str)
        {
            return _json.Beautify(str);
        }
    }
}
