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
                var cookie = HttpContext.Current.User.Identity.Name;
                var userInfo = cookie.Split('|');
                var user = new User()
                {
                    Name = userInfo[0].ToString(),
                    Id = Convert.ToInt64(userInfo[1]),
                    SPId = Convert.ToInt64(userInfo[2]),
                };

                return user;
            }
        }
    }
}