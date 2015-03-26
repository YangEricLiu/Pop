using SE.DSP.Foundation.DataAccess.Entity;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.DataAccess
{
    public class OssRepository : Repository<OssObject, string>, IOssRepository
    {
        public override OssObject GetById(string id)
        {
            var handler = new HttpClientHandler();

            if (!string.IsNullOrWhiteSpace(ConfigHelper.Get("ProxyHost")))
            {
                handler.Proxy = new WebProxy(ConfigHelper.Get("ProxyHost"), int.Parse(ConfigHelper.Get("ProxyPort")));
                handler.UseProxy = true;
            }

            var client = new HttpClient(handler);

            string host = ConfigHelper.Get("DataServiceHost");
            string bucketName = ConfigHelper.Get("DataServiceBucketName");
            Uri url = new Uri(new Uri(host), "simple/" + bucketName + "/" + id);
            byte[] pic = client.GetByteArrayAsync(url).Result;

            return new OssObject(id, pic);
        }

        public override OssObject Add(OssObject entity)
        {
            var handler = new HttpClientHandler();

            if (!string.IsNullOrWhiteSpace(ConfigHelper.Get("ProxyHost")))
            {
                handler.Proxy = new WebProxy(ConfigHelper.Get("ProxyHost"), int.Parse(ConfigHelper.Get("ProxyPort")));
                handler.UseProxy = true;
            }

            var client = new HttpClient(handler);
            string host = ConfigHelper.Get("DataServiceHost");
            string bucketName = ConfigHelper.Get("DataServiceBucketName");
            Uri url = new Uri(new Uri(host), "simple/" + bucketName + "/" + entity.Key);
            client.PutAsync(url, new StreamContent(new MemoryStream(entity.Content))).Wait(10 * 1000);

            return entity;
        }

        public override OssObject Add(IUnitOfWork unitOfWork, OssObject entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(OssObject entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(IUnitOfWork unitOfWork, OssObject entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override void Delete(IUnitOfWork unitOfWork, string id)
        {
            throw new NotImplementedException();
        }
    }
}
