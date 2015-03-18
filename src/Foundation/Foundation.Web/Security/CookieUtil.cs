
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Web;
using System.Web.Security;

namespace SE.DSP.Foundation.Web
{
    public static class CookieUtil
    {
        public static void SaveCookie(User user)
        {
            HttpContext.Current.Session[Constant.SESSIONNAME] = user;

            string userData = JsonHelper.Serialize2String<User>(user);
#if DEBUG
            //var ticket = new FormsAuthenticationTicket(1, user.Name, DateTime.Now, DateTime.Now.AddMinutes(1), false, userData);
            var ticket = new FormsAuthenticationTicket(1, user.Name, DateTime.Now, DateTime.Now.AddHours(24), false, userData);
#else
            var ticket = new FormsAuthenticationTicket(1, user.Name, DateTime.Now, DateTime.Now.AddHours(24), false, userData);
#endif
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            cookie.Path = HttpContext.Current.Request.ApplicationPath.ToLower();

            HttpContext.Current.Response.Cookies.Add(cookie);

            if (user.DemoStatus != 1)
            {
                var usernameCookie = new HttpCookie(Constant.LASTLOGINNAME, user.Name) 
                {
                    Expires = DateTime.UtcNow.AddHours(24), 
                    Path = HttpContext.Current.Request.ApplicationPath 
                };
                HttpContext.Current.Response.Cookies.Add(usernameCookie);
            }
        }

        public static User GetCookie()
        {
            User user = null;

            var userdata = (HttpContext.Current.User.Identity as FormsIdentity).Ticket.UserData;

            if (userdata != null)
            {
                //try
                //{
                user = JsonHelper.Deserialize<User>(userdata);
                //}
                //catch (Exception ex)
                //{
                //    LogHelper.LogError(ex.ToString());

                //    if (HttpContext.Current.Session[Constant.SESSIONNAME] != null)
                //    {
                //        user = HttpContext.Current.Session[Constant.SESSIONNAME] as User;

                //        if (user != null)
                //        {
                //            CookieUtil.SaveCookie(user);
                //        }
                //    }
                //}
            }

            return user;
        }
    }
}
