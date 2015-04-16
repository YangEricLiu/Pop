using System.Linq;
using System.Web.Http;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    [Authorize]
    [RoutePrefix("api/scenepicture")]
    public class ScenePictureController : ApiController
    {
        private readonly IPopClientService clientService;

        public ScenePictureController()
        {
            this.clientService = ServiceProxy<IPopClientService>.GetClient("IPopClientService.EndPoint");
        }

        [HttpGet]
        [Route("upload/{hierarchyId}")]
        public ScenePictureModel[] Upload(long hierarchyId, [FromBody]ScenePictureModel[] logs)
        {
            var dto = logs.Select(s => AutoMapper.Mapper.Map<ScenePictureDto>(s)).ToArray();

            var result = this.clientService.UploadScenePicture(hierarchyId, dto);

            return result.Select(s => AutoMapper.Mapper.Map<ScenePictureModel>(s)).ToArray();
        }
    }
}