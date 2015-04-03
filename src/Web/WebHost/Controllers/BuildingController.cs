using System.Linq;
using System.Web.Http;
using AutoMapper;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    public class BuildingController : ApiController
    {
        private readonly IHierarchyService hierarchyService;

        public BuildingController()
        {
            this.hierarchyService = ServiceProxy<IHierarchyService>.GetClient("IHierarchyService.EndPoint");
        }

        [HttpGet]
        [Route("api/building/{hierarchyId}")]
        public BuildingModel Get(long hierarchyId)
        {
            var tree = this.hierarchyService.GetBuildingById(hierarchyId);

            return Mapper.Map<BuildingDto, BuildingModel>(tree);
        }

        [HttpPost]
        [Route("api/building/delete/{hierarchyId}")]
        public void Delete(long hierarchyId)
        {
            this.hierarchyService.DeleteBuilding(hierarchyId);            
        }

        [HttpPost]
        [Route("api/building/create")]
        public BuildingModel Create([FromBody]BuildingModel park)
        {
            var dto = Mapper.Map<BuildingModel, BuildingDto>(park);

            dto = this.hierarchyService.CreateBuilding(dto);

            return Mapper.Map<BuildingDto, BuildingModel>(dto);
        }

        [HttpPost]
        [Route("api/building/update")]
        public BuildingModel Update([FromBody]BuildingModel park)
        {
            var dto = Mapper.Map<BuildingModel, BuildingDto>(park);

            var result = this.hierarchyService.UpdateBuilding(dto);

            return Mapper.Map<BuildingDto, BuildingModel>(result);
        }
    }
}
