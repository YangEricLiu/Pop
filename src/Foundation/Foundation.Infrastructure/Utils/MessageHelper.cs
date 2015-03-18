using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public class MessageHelper
    {

        private List<Message> list = new List<Message>();

        private static object mutex = new object();

        private static MessageHelper msgHelper = null;

        private MessageHelper()
        {
           list= Initliaize();
        }

        public static MessageHelper GetMessageHelperInstance()
        {
            lock (mutex)
            {
                if (msgHelper == null)
                {
                    msgHelper = new MessageHelper();
                }
                return msgHelper;
            }
        }

        private List<Message> Initliaize()
        {
            XmlDocument doc = new XmlDocument();


            string fileName = Path.Combine(GetPath(), "Config", "tagImportMessage.config");
            doc.Load(fileName);

            var msgList = new List<Message>();

            XmlNodeList xnl = doc.GetElementsByTagName("Message");

            for (int i = 0; i < xnl.Count; i++)
            {
                Message msg = new Message();
                msg.Key = xnl[i].Attributes["Key"].Value;
                msg.Value = xnl[i].Attributes["Value"].Value;
                msg.Module = xnl[i].Attributes["Module"].Value;
                msgList.Add(msg);
            }

          

            return msgList;
        }

        private string GetPath()
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

        public List<string> GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            return list.Where(temp => temp.Key.Equals(key)).Select(temp => temp.Value).ToList();
        }


        public string GetValue(string key, string module)
        {
            if (string.IsNullOrEmpty(key) ||
                string.IsNullOrEmpty(module))
                return null;
            return list.Where(temp => temp.Key.ToUpper().Equals(key.ToUpper()) && temp.Module.Equals(module)).Select(temp => temp.Value).FirstOrDefault();

        }

    }


    internal class Message
    {
        public string Key { set; get; }


        public string Value { set; get; }


        public string Module { set; get; }
    }
}
