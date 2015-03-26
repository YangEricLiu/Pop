using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Web;
using SE.DSP.Pop.BL.API;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    [Authorize]
    public class CustomerController : ApiController
    {
        private readonly IUserService userService;

        public CustomerController()
        {
            this.userService = SE.DSP.Foundation.Web.Wcf.ServiceProxy<IUserService>.GetClient("IUserService.EndPoint");
        }

        [HttpGet]
        [Route("api/customer/logo/{logoId}")]
        public HttpResponseMessage GetLogoById([FromBody]long logoId)
        {
            var logo = this.userService.GetLogoById(logoId);

            if (logo == null || logo.Logo == null || logo.Logo.Length == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }

            var httpResponse = new HttpResponseMessage(); 
            
            httpResponse.Content = new ByteArrayContent(this.GenerateThumbnailImage(logo.Logo));

            httpResponse.Content.Headers.ContentType  = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

            return httpResponse;             
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
