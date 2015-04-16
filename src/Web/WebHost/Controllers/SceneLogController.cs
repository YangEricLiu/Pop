using System.Linq;
using System.Web.Http;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    [Authorize]
    [RoutePrefix("api/scenelog")]
    public class SceneLogController : ApiController
    {
        private readonly IPopClientService clientService;

        public SceneLogController()
        {
            this.clientService = ServiceProxy<IPopClientService>.GetClient("IPopClientService.EndPoint");
        }

        [HttpGet]
        [Route("upload/{hierarchyId}")]
        public SceneLogModel[] Upload(long hierarchyId, [FromBody]SceneLogModel[] logs)
        {
            var dto = logs.Select(s => AutoMapper.Mapper.Map<SceneLogDto>(s)).ToArray();

            var result = this.clientService.UploadSceneLog(hierarchyId, dto);

            return result.Select(s => AutoMapper.Mapper.Map<SceneLogModel>(s)).ToArray();
        }
    }
}