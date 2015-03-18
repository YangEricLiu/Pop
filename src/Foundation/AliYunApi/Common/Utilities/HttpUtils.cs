/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.OpenTableService.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Aliyun.OpenServices.Common.Utilities
{
    /// <summary>
    /// Description of HttpUtils.
    /// </summary>
    internal static class HttpUtils
    {
        public const string Charset = "utf-8";
        public const string Iso88591Charset = "iso-8859-1";

        /// <summary>
        /// Builds the URI parameter string from the request parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GetRequestParameterString(IEnumerable<KeyValuePair<string, string>> parameters, string apiVersion)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool isFirst = true;

            string delimiter1 = null;
            string delimiter2 = null;

            switch (apiVersion)
            {
                case OtsConstants.ApiVersion:
                    delimiter1 = "&";
                    delimiter2 = "=";

                    break;
                case ProtoBufConstant.ProtoBufApiVersion:
                    delimiter1 = "\n";
                    delimiter2 = ":";

                    break;
            }

            foreach (var p in parameters)
            {
                Debug.Assert(!string.IsNullOrEmpty(p.Key), "Null Or empty key is not allowed.");
                if (!isFirst)
                {
                    stringBuilder.Append(delimiter1);
                }
                isFirst = false;
                stringBuilder.Append(p.Key);
                if (p.Value != null)
                {
                    string value = p.Value;

                    if (apiVersion == OtsConstants.ApiVersion)
                    {
                        value = UrlEncode(p.Value, Charset);
                    }

                    stringBuilder.Append(delimiter2).Append(value);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Encodes the URL.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string UrlEncode(string data, string charset)
        {
            Debug.Assert(data != null && !string.IsNullOrEmpty(charset));

            StringBuilder stringBuilder = new StringBuilder(data.Length * 2);
            string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            byte[] bytes = Encoding.GetEncoding(charset).GetBytes(data);
            foreach (char c in bytes)
            {
                if (text.IndexOf(c) != -1)
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    stringBuilder.Append("%").Append(
                        string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)c));
                }
            }
            return stringBuilder.ToString();
        }

        // Convert a text from one charset to another.
        public static string ReEncode(string text, string fromCharset, string toCharset)
        {
            Debug.Assert(text != null);
            var buffer = Encoding.GetEncoding(fromCharset).GetBytes(text);
            return Encoding.GetEncoding(toCharset).GetString(buffer);
        }
    }
}
