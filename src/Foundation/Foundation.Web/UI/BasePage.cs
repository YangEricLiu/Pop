
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace SE.DSP.Foundation.Web
{
    public class BasePage : System.Web.UI.Page
    {
        protected String lang { get; set; }
        protected Language Locale { get; set; }

        protected String _(string key)
        {
            //var sb = new StringBuilder();

            //foreach (var key in keys)
            //{
            //    sb.Append(Schneider.REM.Common.I18nHelper.GetValue(Locale, I18nResourceType.App, key));
            //}

            //return sb.ToString();
            return I18nHelper.GetValue(Locale, I18nResourceType.App, key);
        }

        protected void SetLocaleFromQueryStringOrBrowserSetting()
        {
            lang = Request.QueryString[Constant.LANGUAGEQUERYSTRING];
            if (!String.IsNullOrEmpty(lang))
            {
                Locale = I18nHelper.LocaleStringToEnum(lang);
                return;
            }

            var langs = Request.UserLanguages;
            if (langs.Length == 0) return;


            langs = langs.Select(p=>p.ToLower().Replace("-", "_")).ToArray();
            var sysLangOpts = Enum.GetNames(typeof(Language)).Select(p=>p.ToLower().Replace("-", "_")).ToArray();

            List<String> calcedLangWithoutDist = new List<String>();

            bool matched = false;

            foreach (var tmplang in langs)
            {
                if (sysLangOpts.Any(p=>tmplang.StartsWith(p))) 
                {
                    Locale = I18nHelper.LocaleStringToEnum(tmplang);
                    matched = true;
                    break;
                }
                if (tmplang.Contains("_"))
                {
                    var calced = tmplang.Split('_')[0];
                    if (!calcedLangWithoutDist.Contains(calced))
                    {
                        calcedLangWithoutDist.Add(calced);
                    }
                }
                else
                {
                    var calcTmplang = tmplang.Split(';')[0];
                    var matchLang = sysLangOpts.FirstOrDefault(p => p.StartsWith(calcTmplang));
                    if (matchLang != null)
                    {
                        Locale = I18nHelper.LocaleStringToEnum(matchLang);
                        matched = true;
                        break;
                    }
                }
            }
            
            if (!matched)
            {
                foreach (var tmplang in calcedLangWithoutDist)
                {
                    var matchLang = sysLangOpts.FirstOrDefault(p => p.StartsWith(tmplang));
                    if (matchLang != null)
                    {
                        Locale = I18nHelper.LocaleStringToEnum(tmplang);
                        matched = true;
                    }
                }
            }
        }
    }

}
