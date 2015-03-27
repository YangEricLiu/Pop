using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

using System;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;

namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// A message inspector to insert custom header into app service api invoke messages.
    /// </summary>
    /// <remarks>This class is for internal use only and is not intended to be used directly from your code.</remarks>
    public class CustomHeaderClientMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        /// <summary>
        /// Insert custom header into a request message before it is sent to a service.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            //request id
            MessageHeader<Guid> requestIdHeader = new MessageHeader<Guid>(ServiceWrapperContext.RequestId);
            request.Headers.Add(requestIdHeader.GetUntypedHeader(ServiceContextConstant.REQUESTID, ServiceContextConstant.HEADERNAMESPACE));


            //CurrentLanguage

            MessageHeader<Language> currentLanguage = new MessageHeader<Language>(ServiceWrapperContext.CurrentLanguage);
            request.Headers.Add(currentLanguage.GetUntypedHeader(ServiceContextConstant.LANGUAGE, ServiceContextConstant.HEADERNAMESPACE));
          

            //current user
            MessageHeader<User> currentUserHeader = new MessageHeader<User>(ServiceWrapperContext.CurrentPopUser);
            request.Headers.Add(currentUserHeader.GetUntypedHeader(ServiceContextConstant.CURRENTUSER, ServiceContextConstant.HEADERNAMESPACE));

            return null;
        }
    }
}