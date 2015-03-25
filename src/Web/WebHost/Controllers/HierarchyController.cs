using System.Web.Http;
using AutoMapper;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    public class HierarchyController : ApiController
    {
        private readonly IHierarchyService hierarchyService;

        public HierarchyController()
        {
            this.hierarchyService = ServiceProxy<IHierarchyService>.GetClient("IHierarchyService.EndPoint");
        }

        [HttpGet]
        [Route("api/hierarchy/{customerId}")]
        public HierarchyModel Get(long customerId)
        {
            var tree = this.hierarchyService.GetHierarchyTree(customerId);

            return Mapper.Map<HierarchyDto, HierarchyModel>(tree);
        }

        [HttpPost]
        [Route("api/hierarchy/delete/{hierarchyId}")]
        public void Delete(long hierarchyId)
        {
            this.hierarchyService.DeleteHierarchy(hierarchyId, false);            
        }

        [HttpPost]
        [Route("api/hierarchy/create")]
        public HierarchyModel Post([FromBody]HierarchyModel hierarchy)
        {
            var dto = Mapper.Map<HierarchyModel, HierarchyDto>(hierarchy);

            dto = this.hierarchyService.CreateHierarchy(dto);

            return Mapper.Map<HierarchyDto, HierarchyModel>(dto);
        }

        [HttpPost]
        [Route("api/hierarchy/update")]
        public void Put([FromBody]HierarchyModel hierarchy)
        {
            var dto = Mapper.Map<HierarchyModel, HierarchyDto>(hierarchy);

            this.hierarchyService.UpdateHierarchy(dto);
        }
    }
}
