using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SE.DSP.Foundation.Infrastructure.Utils;

namespace SE.DSP.Pop.Web.WebHost.Common
{
    public static class WebContext
    {
        public static Guid RequestId
        {
            get
            {
                ////not web invoking
                if (HttpContext.Current == null) 
                {
                    return Guid.NewGuid();
                }
                else
                {
                    ////web invoking
                    return (Guid)(HttpContext.Current.Items[ServiceContextConstant.REQUESTID] ?? Guid.NewGuid());
                }
            }

            internal set
            {
                HttpContext.Current.Items[ServiceContextConstant.REQUESTID] = value;
            }
        }

        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.User == null || HttpContext.Current.User.Identity == null || string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return null;
                }

                var cookie = HttpContext.Current.User.Identity.Name;

                if (!cookie.Contains("|"))
                {
                    return null;
                }

                var userInfo = cookie.Split('|');

                if (userInfo.Length != 3)
                {
                    return null;
                }

                User user = null; 
                long userId = 0, spId = 0;

                if (long.TryParse(userInfo[1], out userId) && long.TryParse(userInfo[2], out spId))
                {
                    user = new User()
                    {
                        Name = userInfo[0].ToString(),
                        Id = Convert.ToInt64(userInfo[1]),
                        SPId = Convert.ToInt64(userInfo[2]),
                    };
                }

                return user;
            }
        }
    }
}