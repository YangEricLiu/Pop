/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// Description of ProtoBufRequestSigner.
    /// </summary>
    internal class ProtoBufRequestSigner : IRequestSigner
    {
        public void Sign(ServiceRequest request, ServiceCredentials credentials)
        {
            AddRequiredParameters(request.Parameters,
                                  request.ResourcePath,
                                  request.Method.ToString().ToUpper(),
                                  credentials,
                                  ServiceSignature.Create(),
                                  DateTime.UtcNow);
        }

        private static void AddRequiredParameters(IDictionary<string, string> parameters,
                                                  string action,
                                                  string requestMethod,
                                                  ServiceCredentials credentials,
                                                  ServiceSignature signer,
                                                  DateTime timestamp)
        {
            parameters[ProtoBufConstant.HeaderDate] = DateUtils.FormatRfc822Date(timestamp);
            parameters[ProtoBufConstant.HeaderAccessId] = credentials.AccessId;
            parameters[ProtoBufConstant.HeaderApiVersion] = ProtoBufConstant.ProtoBufApiVersion;

            using (var md5Hash = MD5.Create())
            {
                var md5Content = md5Hash.ComputeHash(parameters[ProtoBufConstant.ParameterContent].Split(ProtoBufConstant.Separator).Select(item => Convert.ToByte(item)).ToArray());

                parameters[ProtoBufConstant.HeaderContentMd5] = Convert.ToBase64String(md5Content);

            }

            var signature = GetSignature(credentials, signer, action, requestMethod, parameters);
            parameters[ProtoBufConstant.HeaderSignature] = signature;
        }

        private static string GetSignature(ServiceCredentials credentials,
                                           ServiceSignature signer,
                                           string action,
                                           string requestMethod,
                                           IDictionary<string, string> parameters)
        {
            // The parameters to compute signature need to be sorted.
            var signatureData =
                string.Format(CultureInfo.InvariantCulture, "/{0}\n{1}\n\n{2}\n", action, requestMethod,
                              HttpUtils.GetRequestParameterString(parameters.Where(para => para.Key.Contains(ProtoBufConstant.HeaderPrefix)).OrderBy(e => e.Key, StringComparer.Ordinal), ProtoBufConstant.ProtoBufApiVersion));

            var signature = signer.ComputeSignature(credentials.AccessKey, signatureData);

            //LogSign(action, signatureData, signature, parameters);

            return signature;
        }

        //private static fastJSON.JSONParamters _jsonParams = new fastJSON.JSONParamters() { UseExtensions = true, UsingGlobalTypes = false, UseUTCDateTime = false, };
        //private static fastJSON.JSON _json = fastJSON.JSON.Instance;
        //private static object temp = new object();

        //private static void LogSign(string action, string signatureData, string signature, IDictionary<string, string> parameters)
        //{
        //    string logFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log/ots.log");

        //    lock (temp)
        //    {
        //        System.IO.FileInfo logFile = new System.IO.FileInfo(logFilePath);
        //        if (!logFile.Directory.Exists)
        //            logFile.Directory.Create();

        //        var __logwriter = new System.IO.StreamWriter(logFilePath, true);
        //        DateTime now = DateTime.Now;
        //        string time = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //        __logwriter.WriteLine("{0}\taction:{1}", time, action);
        //        __logwriter.WriteLine("{0}\tparameters:{1}", time, _json.ToJSON(parameters, _jsonParams));
        //        __logwriter.WriteLine("{0}\tsignatureData:{1}", time, signatureData);
        //        __logwriter.WriteLine("{0}\tsignature:{1}\n", time, signature);
        //        __logwriter.Flush();
        //        __logwriter.Close();
        //    }
        //}
    }
}
