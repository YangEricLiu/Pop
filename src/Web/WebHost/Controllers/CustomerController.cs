using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Web;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    [Authorize]
    public class CustomerController : ApiController
    {
        private readonly ICustomerService customerService;

        public CustomerController()
        {
            this.customerService = SE.DSP.Foundation.Web.Wcf.ServiceProxy<ICustomerService>.GetClient("ICustomerService.EndPoint");
        }

        [HttpGet]
        [Route("api/customer/logo/{logoId}")]
        public HttpResponseMessage GetLogoById(long logoId)
        {
            var logo = this.customerService.GetLogoById(logoId);

            if (logo == null || logo.Logo == null || logo.Logo.Length == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }

            var httpResponse = new HttpResponseMessage(); 
            
            httpResponse.Content = new ByteArrayContent(this.GenerateThumbnailImage(logo.Logo));

            httpResponse.Content.Headers.ContentType  = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

            return httpResponse;
        }

        [HttpGet]
        [Route("api/user/{userId}/customers")]
        public CustomerListItemModel[] GetCustomersByUserId(long userId)
        {
            var customerList = this.customerService.GetCustomersByUserId(userId);

            return customerList.Select(c => AutoMapper.Mapper.Map<CustomerListItemModel>(c)).ToArray();
        }

        [HttpPost]
        [Route("api/customer/create")]
        public CustomerModel CreateCustomer([FromBody]CustomerModel customer)
        {
            var resut = this.customerService.CreateCustomer(AutoMapper.Mapper.Map<CustomerDto>(customer));

            return AutoMapper.Mapper.Map<CustomerModel>(resut);
        }

        [HttpPost]
        [Route("api/customer/update")]
        public CustomerModel UpdateCustomer([FromBody]CustomerModel customer)
        {
            var resut = this.customerService.UpdateCustomer(AutoMapper.Mapper.Map<CustomerDto>(customer));

            return AutoMapper.Mapper.Map<CustomerModel>(resut);
        }

        [HttpPost]
        [Route("api/customer/delete/{customerId}")]
        public void DeleteCustomer(long customerId)
        {
            this.customerService.DeleteCustomer(customerId);
        }

        private byte[] GenerateThumbnailImage(byte[] logo)
        {
            var targetWidth = Convert.ToInt32(ConfigurationManager.AppSettings[ConfiguratgionKey.LOGOWIDTH]);
            var targetHeight = Convert.ToInt32(ConfigurationManager.AppSettings[ConfiguratgionKey.LOGOHEIGHT]);

            using (var oStream = new MemoryStream(logo))
            {
                using (var oimg = Image.FromStream(oStream))
                {
                    var tImg = ImageHelper.GenerateThumbnailImage(oimg, new Size(targetWidth, targetHeight));

                    using (MemoryStream tStream = new MemoryStream())
                    {
                        tImg.Save(tStream, ImageFormat.Png);

                        tImg.Dispose();

                        var tLogo = tStream.ToArray();

                        return tLogo;
                    }
                }
            }
        }
    }
}
