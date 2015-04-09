using System.Web.Http;
using AutoMapper;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    [Authorize]
    public class ParkController : ApiController
    {
        private readonly IHierarchyService hierarchyService;

        public ParkController()
        {
            this.hierarchyService = ServiceProxy<IHierarchyService>.GetClient("IHierarchyService.EndPoint");
        }

        [HttpGet]
        [Route("api/park/{hierarchyId}")]
        public ParkModel Get(long hierarchyId)
        {
            var tree = this.hierarchyService.GetParkById(hierarchyId);

            return Mapper.Map<ParkDto, ParkModel>(tree);
        }

        [HttpPost]
        [Route("api/park/delete/{hierarchyId}")]
        public void Delete(long hierarchyId)
        {
            this.hierarchyService.DeletePark(hierarchyId);            
        }

        [HttpPost]
        [Route("api/park/create")]
        public ParkModel Create([FromBody]ParkModel park)
        {
            var dto = Mapper.Map<ParkModel, ParkDto>(park);

            dto = this.hierarchyService.CreatePark(dto);

            return Mapper.Map<ParkDto, ParkModel>(dto);
        }

        [HttpPost]
        [Route("api/park/update")]
        public ParkModel Update([FromBody]ParkModel park)
        {
            var dto = Mapper.Map<ParkModel, ParkDto>(park);

            var result = this.hierarchyService.UpdatePark(dto);

            return Mapper.Map<ParkDto, ParkModel>(result);
        }
    }
}
