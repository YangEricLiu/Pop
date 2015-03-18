using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public class OssHelper
    {
        public static byte[] GetJazzObject(string key)
        {
            HttpClientHandler handler = new HttpClientHandler();
            if (!string.IsNullOrWhiteSpace(ConfigHelper.Get("ProxyHost")))
            {
                handler.Proxy = new WebProxy(ConfigHelper.Get("ProxyHost"), int.Parse(ConfigHelper.Get("ProxyPort")));
                handler.UseProxy = true;
            }
            HttpClient client = new HttpClient(handler);

            string host = ConfigHelper.Get("DataServiceHost");
            string bucketName = ConfigHelper.Get("DataServiceBucketName");
            Uri url = new Uri(new Uri(host), "simple/" + bucketName + "/" + key);
            byte[] pic = client.GetByteArrayAsync(url).Result;

            return pic;
        }

        public static byte[] GetStaticObject(string key)
        {
            HttpClientHandler handler = new HttpClientHandler();
            if (!string.IsNullOrWhiteSpace(ConfigHelper.Get("ProxyHost")))
            {
                handler.Proxy = new WebProxy(ConfigHelper.Get("ProxyHost"), int.Parse(ConfigHelper.Get("ProxyPort")));
                handler.UseProxy = true;
            }
            HttpClient client = new HttpClient(handler);

            string host = ConfigHelper.Get("DataServiceHost");
            string bucketName = ConfigHelper.Get("StaticBucketName");
            Uri url = new Uri(new Uri(host), "simple/" + bucketName + "/" + key);
            byte[] pic = client.GetByteArrayAsync(url).Result;

            return pic;
        }


        public static void PutJazzObject(string key, byte[] content)
        {

            HttpClientHandler handler = new HttpClientHandler();
            if (!string.IsNullOrWhiteSpace(ConfigHelper.Get("ProxyHost")))
            {
                handler.Proxy = new WebProxy(ConfigHelper.Get("ProxyHost"), int.Parse(ConfigHelper.Get("ProxyPort")));
                handler.UseProxy = true;
            }
            HttpClient client = new HttpClient(handler);
            string host = ConfigHelper.Get("DataServiceHost");
            string bucketName = ConfigHelper.Get("DataServiceBucketName");
            Uri url = new Uri(new Uri(host), "simple/" + bucketName + "/" + key);
            client.PutAsync(url, new StreamContent(new MemoryStream(content))).Wait(10 * 1000);



        }
    }
}
