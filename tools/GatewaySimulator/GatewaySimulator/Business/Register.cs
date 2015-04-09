using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewaySimulator.Business
{
    public static class RegisterBusiness
    {
        public static void RegesterCommand(string name, string mac)
        {
            var errorMap = new Dictionary<int, string> 
            { 
                {1,"Invalid name"},
                {2,"Invalid code"},
                {3,"Customer not exist"},
                {4,"Box already exist"},
                {5,"Box not exist"},
            };

            var response = Register(name, mac);

            var validation = ValidateResponse(response, errorMap, 4);

            if (validation.HasValue && validation.Value == 4)
            {
                Console.WriteLine("Box already exist, replace? Y,N");
                var input = Console.ReadLine();
                if (input.Trim().ToLower().StartsWith("y"))
                {
                    var r2 = Replace(name, mac);
                    var v2 = ValidateResponse(r2, errorMap);
                }
            }
        }

        public static dynamic Replace(string name, string mac)
        {
            var url = string.Format("{0}{1}", ConfigurationManager.AppSettings["WebHost"], ConfigurationManager.AppSettings["ReplaceUrl"]);

            string parameter = string.Format("boxname={0}&boxmac={1}", name, mac);
            string absolute = string.Format("{0}?{1}", url, parameter);

            var responseText = HttpHelper.Get(absolute);

            dynamic response = JObject.Parse(responseText);

            return response;
        }

        public static dynamic Register(string name, string mac)
        {
            var url = string.Format("{0}{1}", ConfigurationManager.AppSettings["WebHost"], ConfigurationManager.AppSettings["RegisterUrl"]);

            string parameter = string.Format("boxname={0}&boxmac={1}", name, mac);
            string absolute = string.Format("{0}?{1}", url, parameter);

            var responseText = HttpHelper.Get(absolute);

            dynamic response = JObject.Parse(responseText);

            return response;
        }

        public static int? ValidateResponse(ResponseMessage response, Dictionary<int, string> errorMap, int? specialCode = null)
        {
            var code = Convert.ToInt32(response.Error);

            if (specialCode.HasValue && specialCode.Value == code)
            {
                return specialCode;
            }

            if (code == 0)
            {
                var result = response.Result;
                Console.WriteLine(result.GetType());
            }
            else if (code == -1)
            {
                Console.WriteLine("server error");
            }
            else
            {
                var message = errorMap[code];
                Console.WriteLine("error: {0}", message);
            }

            return null;
        }
    }
}
