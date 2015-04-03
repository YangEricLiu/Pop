using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GatewaySimulator
{
    public static class HttpHelper
    {
        public static string Get(string url)
        {
            return MakeHttpRequest("GET", url, null);
        }

        public static string Post(string url, string body)
        {
            return MakeHttpRequest("POST", url, body);
        }

        public static string Put(string url, string body)
        {
            return MakeHttpRequest("PUT", url, body);
        }

        public static string Delete(string url, string body)
        {
            return MakeHttpRequest("DELETE", url, body);
        }

        private static string MakeHttpRequest(string method, string url, string body)
        {
            HttpClientHandler handler = new HttpClientHandler();

            var proxyHost = ConfigurationManager.AppSettings["ProxyHost"];
            var proxyPort = ConfigurationManager.AppSettings["ProxyPort"];

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ProxyHost"]))
            {
                handler.Proxy = new WebProxy(proxyHost, Convert.ToInt32(proxyPort));
                handler.UseProxy = true;
            }

            HttpClient client = new HttpClient(handler);

            client.Timeout = TimeSpan.FromMinutes(1);
            HttpResponseMessage output;
            if (method == "PUT")
            {
                output = client.PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json")).Result;
            }
            else if (method == "POST")
            {
                output = client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json")).Result;
            }
            else if (method == "DELETE")
            {
                output = client.SendAsync(new HttpRequestMessage()
                {
                    Method = HttpMethod.Delete,
                    Content = new StringContent(body, UTF8Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(url)
                }).Result;
            }
            else
            {
                output = client.GetAsync(url).Result;
            }

            return output.Content.ReadAsStringAsync().Result;

            /*var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.Timeout = 60000 * 5;
            request.ContentType = "application/json";
            request.ContentLength = 0;
            if (!String.IsNullOrEmpty(ConfigHelper.Get("ProxyHost")))
                request.Proxy = new WebProxy(ConfigHelper.Get("ProxyHost"), Convert.ToInt32(ConfigHelper.Get("ProxyPort")));

            if (!String.IsNullOrEmpty(body))
            {
                var requestBody = Encoding.UTF8.GetBytes(body);
                request.ContentLength = requestBody.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(requestBody, 0, requestBody.Length);
                }
            }
            string output = "";
            using (var response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    output = sr.ReadToEnd();
                }
            }

            return output;*/
        }
    }
}
