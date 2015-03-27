using System;
using System.Web;
using System.Web.Security;

using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using System.Security.Claims;

namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// Represents the web service api wrapper context
    /// </summary>
    public static class ServiceWrapperContext
    {
        /// <summary>
        /// Gets or sets the request GUID for current request.
        /// </summary>
        /// <exception cref="Exception">Not access web api service wrapper from http application.</exception>
        public static Guid RequestId
        {
            get
            {
                if (HttpContext.Current == null) //not web invoking
                {
                    return Guid.NewGuid();
                }
                else //web invoking
                {
                    return (Guid)(HttpContext.Current.Items[ServiceContextConstant.REQUESTID] ?? Guid.NewGuid());
                }
            }

            internal set
            {
                HttpContext.Current.Items[ServiceContextConstant.REQUESTID] = value;
            }
        }


        public static Language CurrentLanguage
        {
            get
            {
                if (HttpContext.Current == null)
                    return Language.ZH_CN;

                string queryString = HttpContext.Current.Request.QueryString[Constant.LANGUAGEQUERYSTRING];

                if (!string.IsNullOrEmpty(queryString))
                {
                    return I18nHelper.LocaleStringToEnum(queryString);
                }

                var lang = HttpContext.Current.Request.Cookies["CurrentLanguage"];

                if (lang!=null&&
                    !String.IsNullOrEmpty(lang.Value))
                { 
                    return I18nHelper.LocaleStringToEnum(lang.Value);
                }

                else
                {
                    var request = HttpContext.Current.Request;
                    if (request != null && request.UserLanguages != null && request.UserLanguages.Length > 0)
                    {
                        var firstLan = request.UserLanguages[0];
                        if (firstLan.ToUpper().Contains("EN"))
                        {
                            return I18nHelper.LocaleStringToEnum("en-US");
                        }
                    }
                    return I18nHelper.LocaleStringToEnum("zh-CN");
                }
            }
        }

        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.User!=null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
                        if (identity != null)
                        {
                            var user = new User();
                            String remuserId = null, realname = null;
                            long? remUserVersion = null, remSpId = null;
                            int? remDemostatus = null;
                            foreach (var claim in identity.Claims)
                            {
                                if (claim.Type == System.IdentityModel.Claims.ClaimTypes.Name) user.Name = claim.Value;
                                if (claim.Type == STSConstant.IPSTSName) user.IPSTS = claim.Value;

                                if (user.IPSTS == STSConstant.IPSTSRem)
                                {
                                    if (claim.Type == STSConstant.RemUserId) remuserId = claim.Value;
                                    else if (claim.Type == STSConstant.RemUserRealName) realname = claim.Value;
                                    else if (claim.Type == STSConstant.RemUserVersion) remUserVersion = Convert.ToInt64(claim.Value);
                                    else if (claim.Type == STSConstant.RemSpId) remSpId = Convert.ToInt64(claim.Value);
                                    else if (claim.Type == STSConstant.RemDemoStatus) remDemostatus = Convert.ToInt32(claim.Value);
                                    //else if (claim.ClaimType == STSConstant.RemUserType) remUserType = Convert.ToInt64(claim.Value);
                                }
                            }
                            if (user.IPSTS == STSConstant.IPSTSRem)
                            {
                                user.Id = Convert.ToInt32(remuserId);
                                user.SPId = Convert.ToInt32(remSpId);
                                user.DemoStatus = Convert.ToInt32(remDemostatus);
                                //user.RealName = realname;
                                //user.Version = remUserVersion;
                                //user.UserType = remUserType.Value;
                            }
                            else
                            {
                                // Retrieve data from user table with IPSTSName and UserName
                            }
                            return user;
                        }
                        else
                        {
                            var user = CookieUtil.GetCookie();
                            // TODO: seems in very rare senario, we cant get any user, but current user is logged in. 
                            // So just use the code below to get rid of exception.

                            // A possible reason is, when set FormIdentity, but the userdata is not saved to cookie.
                            // When try to deserilize current user, we will run into an exception.
                            if (user == null)
                            {
                                user = new User();
                                user.Id = 1;
                                user.Name = "PlatformAdmin";
                                user.IPSTS = STSConstant.IPSTSRem;
                            }
                            return user;
                        }
                    }
                    else
                    {
                        //TODO: remove this code when mobile 0.1 is finished
                        User user = new User();
                        user.Id = 1;
                        user.Name = "PlatformAdmin";
                        user.IPSTS = STSConstant.IPSTSRem;
                        return user;
                    }
                }
                else
                {
                    //TODO: remove this code when mobile 0.1 is finished
                    User user = new User();
                    user.Id = 1;
                    user.Name = "PlatformAdmin";
                    user.IPSTS = STSConstant.IPSTSRem;
                    return user;               
                }
            }
        }

        public static User CurrentPopUser
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        var identity = HttpContext.Current.User.Identity as FormsIdentity;
                        if (identity != null)
                        {
                            var user = new User
                            {
                                Name = identity.Name,
                                Id = 1,
                                SPId = 1
                            };

                            return user;
                           
                        }
                        else
                        {
                            var user = CookieUtil.GetCookie();
                            // TODO: seems in very rare senario, we cant get any user, but current user is logged in. 
                            // So just use the code below to get rid of exception.

                            // A possible reason is, when set FormIdentity, but the userdata is not saved to cookie.
                            // When try to deserilize current user, we will run into an exception.
                            if (user == null)
                            {
                                user = new User();
                                user.Id = 1;
                                user.Name = "PlatformAdmin";
                                user.IPSTS = STSConstant.IPSTSRem;
                            }
                            return user;
                        }
                    }
                    else
                    {
                        //TODO: remove this code when mobile 0.1 is finished
                        User user = new User();
                        user.Id = 1;
                        user.Name = "PlatformAdmin";
                        user.IPSTS = STSConstant.IPSTSRem;
                        return user;
                    }
                }
                else
                {
                    //TODO: remove this code when mobile 0.1 is finished
                    User user = new User();
                    user.Id = 1;
                    user.Name = "PlatformAdmin";
                    user.IPSTS = STSConstant.IPSTSRem;
                    return user;
                }
            }
        }

        public static string SessionId
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null && !String.IsNullOrEmpty(HttpContext.Current.Session.SessionID))
                    return HttpContext.Current.Session.SessionID;

                return String.Empty;
            }
        }
    }
}