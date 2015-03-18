using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace SE.DSP.Foundation.Infrastructure.ActionExtension
{
     internal class ExtensionCfgsParser
    {

        private static ActionExtensionCfgs actionExtensionCfgs = null;

        public static ActionExtensionCfgs ActionExtensionCfgCollection
        {
            get { return actionExtensionCfgs; }
         
        }


        private static object lockObj = new object();

        /// <summary>
        /// load cfg file
        /// </summary>
        /// <param name="fileName">virtual path or absolute path</param>
        /// <returns>ActionExtensionCfgs </returns>
        public static void LoadFile(string fileName)
        {
            if (actionExtensionCfgs != null)
                return;

            lock (lockObj)
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    LogHelper.LogError(string.Format("load actionextensioncfgs  file failed.param is null or empty."));
                    return;
                }
                string absolutePath = fileName;

                if(HttpContext.Current!=null)
                {
                    absolutePath = HttpContext.Current.Server.MapPath(fileName);
                }
                 
                if (!File.Exists(absolutePath))
                {
                    LogHelper.LogError(string.Format("load fileName=[{0}] failed. file does not find.", absolutePath));
                    return;
                }

                XmlSerializer serializer = XmlSerializer.FromTypes(new Type[] { typeof(ActionExtensionCfgs) })[0];

                using (FileStream fs = File.Open(absolutePath, FileMode.OpenOrCreate,FileAccess.Read))
                {
                    actionExtensionCfgs = serializer.Deserialize(fs) as ActionExtensionCfgs;
                }

                return;
            }
        }



    
    }
}
