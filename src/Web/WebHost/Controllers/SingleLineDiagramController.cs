using System.Linq;
using System.Web.Http;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    [Authorize]
    public class SingleLineDiagramController : ApiController
    {
        private readonly IPopClientService popClientService;

        public SingleLineDiagramController()
        { 
            this.popClientService = ServiceProxy<IPopClientService>.GetClient("IPopClientService.EndPoint");
        }

        [HttpGet]
        [Route("api/building/{hierarchyId}/singlelinediagrams")]
        public SingleLineDiagramModel[] GetSingleDiagramsByBuildingId(long hierarchyId)
        {
            var result = this.popClientService.GetSingleLineDiagramByHierarchyId(hierarchyId);

            return result.Select(r => AutoMapper.Mapper.Map<SingleLineDiagramDto, SingleLineDiagramModel>(r)).ToArray();
        }

        [HttpGet]
        [Route("api/singlelinediagram/{id}")]
        public SingleLineDiagramModel Get(long id)
        {
            var result = this.popClientService.GetSingleLineDiagramById(id);

            return AutoMapper.Mapper.Map<SingleLineDiagramDto, SingleLineDiagramModel>(result);
        }

        [HttpPost]
        [Route("api/singlelinediagram/create")]
        public SingleLineDiagramModel Create([FromBody]SingleLineDiagramModel model)
        {
            var result = this.popClientService.CreateSingleLineDiagram(AutoMapper.Mapper.Map<SingleLineDiagramDto>(model));

            return AutoMapper.Mapper.Map<SingleLineDiagramDto, SingleLineDiagramModel>(result);
        }

        [HttpPost]
        [Route("api/singlelinediagram/update")]
        public SingleLineDiagramModel Update([FromBody]SingleLineDiagramModel model)
        {
            var result = this.popClientService.UpdateSingleLineDiagram(AutoMapper.Mapper.Map<SingleLineDiagramDto>(model));

            return AutoMapper.Mapper.Map<SingleLineDiagramDto, SingleLineDiagramModel>(result);
        }

        [HttpPost]
        [Route("api/singlelinediagram/delete/{id}")]
        public void Delete(long id)
        {
            this.popClientService.DeleteSingleLineDiagramById(id); 
        }
    }
}
