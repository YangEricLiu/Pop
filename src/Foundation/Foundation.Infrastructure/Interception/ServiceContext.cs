using System;
using System.Linq;
using System.ServiceModel;

using System.Runtime.Remoting.Messaging;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Constant;

namespace SE.DSP.Foundation.Infrastructure.Interception
{
    /// <summary>
    /// Represents the app service api context
    /// 
    /// 
    /// modified by mike 20140526 add "Language" property
    /// </summary>
    public static class ServiceContext
    {
        [ThreadStatic]
        private static Guid _requestId;

        [ThreadStatic]
        private static User _currentUser;


        private static Language _language;

        /// <summary>
        ///  added by mike on 20140526 
        /// </summary>
        public static Language Language
        {
            get
            {
                if (OperationContext.Current == null)
                    return Language.ZH_CN;
                bool res = OperationContext.Current.IncomingMessageHeaders.Any(temp => temp.Name.Equals(ServiceContextConstant.LANGUAGE));
                if (!res)
                    return Language.ZH_CN;
                _language = OperationContext.Current.IncomingMessageHeaders.GetHeader<Language>(ServiceContextConstant.LANGUAGE, ServiceContextConstant.HEADERNAMESPACE);   
                return _language;
            }
        }

        /// <summary>
        /// Set RequestId, CurrentUser.
        /// </summary>
        /// <param name="requestId">Request id.</param>
        /// <param name="currentUser">Current user.</param>
        public static void SetServiceContext(Guid requestId, User currentUser)
        {
            _requestId = requestId;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Gets the request id of this invoking.
        /// </summary>
        public static Guid RequestId
        {
            get
            {
                if (OperationContext.Current == null) //UT invoke
                {
                    return _requestId;
                }
                else //WCF invoke
                {
                   _requestId= OperationContext.Current.IncomingMessageHeaders.Any(header => header.Name == ServiceContextConstant.REQUESTID) ? OperationContext.Current.IncomingMessageHeaders.GetHeader<Guid>(ServiceContextConstant.REQUESTID, ServiceContextConstant.HEADERNAMESPACE) : Guid.Empty;

                   return _requestId;
                }
            }
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        public static User CurrentUser
        {
            get
            {
                if (OperationContext.Current == null) //UT invoke
                {
                    var user = CallContext.LogicalGetData(ServiceContextConstant.CURRENTUSER);

                     if (user == null)
                     {
                         if (_currentUser == null)
                         {
                             _currentUser = new User
                             {
                                 SPId = 1,
                                 Name = "",
                             };
                         }
                     }
                    else
                     {
                         _currentUser = (User)user;
                     }

                   

                    return _currentUser;
                }
                else //WCF invoke
                {
                    _currentUser= (User)OperationContext.Current.IncomingMessageHeaders.GetHeader<User>(ServiceContextConstant.CURRENTUSER, ServiceContextConstant.HEADERNAMESPACE);

                    return _currentUser;
                }
            }
        }
    }
}