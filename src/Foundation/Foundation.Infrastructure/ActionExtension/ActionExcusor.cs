using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SE.DSP.Foundation.Infrastructure.ActionExtension
{
    public  static class ActionExcusor
    {
        private static void Init(string fileName)
        {
            ExtensionCfgsParser.LoadFile(fileName);
        }

        private static string GetPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                AppPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
                    AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            return AppPath;
        }
     

        /// <summary>
        /// run extensions for action
        /// </summary>
        /// <param name="path">the key from config file</param>
        /// <param name="isInTransaction"></param>
        /// <param name="positionType">action before or after</param>
        /// <param name="hostDict">params dictionary(key=paramname,value=paramValue)</param>
        /// <param name="configFileName">default file name is null</param>
        public static void Fire(string path,bool isInTransaction,PositionType positionType,Dictionary<string,string> hostDict,string configFileName=null)
        {

            if(string.IsNullOrEmpty(configFileName))
            {
                if(ExtensionCfgsParser.ActionExtensionCfgCollection==null)
                {
                    string fileName = Path.Combine(GetPath(), "Config", "action.extensions.config");

                   // var fileName = @"Config/actionExtension.config";
                    Init(fileName);
                }
            }
            else
            {
                Init(configFileName);
            }

            ActionExtensionCfgs cfgs = ExtensionCfgsParser.ActionExtensionCfgCollection;

            List<ActionCfg> list = new List<ActionCfg>();
            try
            {
                //find action list of one module
                list = cfgs.ActionExtensionCfgCollection.Single(temp => temp.Path.Equals(path)).ActionCfgs.OrderBy(a => a.Order).ToList();
            }
            catch(Exception ex)
            {
                LogHelper.LogException(ex);
                return;
             //   throw ex;
            }
            DefaultActionContext context = new DefaultActionContext();
            context.SetHostValue(hostDict);
            context.IsInTransaction = isInTransaction;
            context.PositionType = positionType;

            foreach(var temp in list)
            {
                try
                {
                    Type type = Type.GetType(temp.ActionType);
                    ActionExtensionBase instance = Activator.CreateInstance(type) as ActionExtensionBase;
                    instance.Run(context);
                }
                catch(Exception ex)
                {
                    LogHelper.LogException(ex);
                   // throw ex; //todo: exception need redefine
                }
            }

        }

    }

}
