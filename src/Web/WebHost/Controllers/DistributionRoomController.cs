using System.Web.Http;
using AutoMapper;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    public class DistributionRoomController : ApiController
    {
        private readonly IHierarchyService hierarchyService;

        public DistributionRoomController()
        {
            this.hierarchyService = ServiceProxy<IHierarchyService>.GetClient("IHierarchyService.EndPoint");
        }

        [HttpGet]
        [Route("api/distributionRoom/{hierarchyId}")]
        public DistributionRoomModel Get(long hierarchyId)
        {
            var tree = this.hierarchyService.GetDistributionRoomById(hierarchyId);

            return Mapper.Map<DistributionRoomDto, DistributionRoomModel>(tree);
        }

        [HttpPost]
        [Route("api/distributionRoom/delete/{hierarchyId}")]
        public void Delete(long hierarchyId)
        {
            this.hierarchyService.DeleteDistributionRoom(hierarchyId);            
        }

        [HttpPost]
        [Route("api/distributionRoom/create")]
        public DistributionRoomModel Create([FromBody]DistributionRoomModel DistributionRoom)
        {
            var dto = Mapper.Map<DistributionRoomModel, DistributionRoomDto>(DistributionRoom);

            dto = this.hierarchyService.CreateDistributionRoom(dto);

            return Mapper.Map<DistributionRoomDto, DistributionRoomModel>(dto);
        }

        [HttpPost]
        [Route("api/DistributionRoom/update")]
        public DistributionRoomModel Update([FromBody]DistributionRoomModel DistributionRoom)
        {
            var dto = Mapper.Map<DistributionRoomModel, DistributionRoomDto>(DistributionRoom);

            var result = this.hierarchyService.UpdateDistributionRoom(dto);

            return Mapper.Map<DistributionRoomDto, DistributionRoomModel>(result);
        }
    }
}
