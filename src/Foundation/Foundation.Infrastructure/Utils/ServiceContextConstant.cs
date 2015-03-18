/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: ServiceContextConstant.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Service Context Constant
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System;
using System.ServiceModel;
using System.Web;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// Service context constant for custom header.
    /// </summary>
    public static class ServiceContextConstant
    {
        /// <summary>
        /// Custom header namespace.
        /// </summary>
        public const string HEADERNAMESPACE = "http://REM.Schneider.com";

        /// <summary>
        /// Request id header item name.
        /// </summary>
        public const string REQUESTID = "RequestId";

        /// <summary>
        /// Current user header item name.
        /// </summary>
        public const string CURRENTUSER = "CurrentUser";

        /// <summary>
        /// Session id header item name.
        /// </summary>
        public const string SESSIONID = "SessionId";


        public const string LANGUAGE = "Language";

        /// <summary>
        /// Get reqeust id.
        /// </summary>
        /// <returns></returns>
        public static Guid GetRequestId()
        {
            Guid requestId;

            if (HttpContext.Current == null) //UT/Backend/App service invoke
            {
                if (OperationContext.Current == null) //UT/Backend
                {
                    requestId = Guid.NewGuid();
                }
                else //App service invoke
                {
                    if (OperationContext.Current.RequestContext.RequestMessage.State == System.ServiceModel.Channels.MessageState.Closed)
                    {
                        requestId = Guid.NewGuid();
                    }
                    else
                    {
                        if (OperationContext.Current.IncomingMessageHeaders.FindHeader(ServiceContextConstant.REQUESTID, ServiceContextConstant.HEADERNAMESPACE) == -1) //add servive refer
                        {
                            requestId = Guid.NewGuid();
                        }
                        else
                        {
                            requestId = OperationContext.Current.IncomingMessageHeaders.GetHeader<Guid>(ServiceContextConstant.REQUESTID, ServiceContextConstant.HEADERNAMESPACE);
                        }
                    }
                }
            }
            else //web request/web service invoke
            {
                object requestIdObj = HttpContext.Current.Items[ServiceContextConstant.REQUESTID];

                if (requestIdObj == null) //sts
                {
                    requestId = Guid.NewGuid();
                }
                else
                {
                    requestId = (Guid)requestIdObj;
                }
            }

            return requestId;
        }
    }
}