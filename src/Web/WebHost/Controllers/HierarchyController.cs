using AutoMapper;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    //[RoutePrefix("hierarchy")]
    public class HierarchyController : ApiController
    {
        private readonly IHierarchyService HierarchyService;

        public HierarchyController()
        {
            HierarchyService = ServiceProxy<IHierarchyService>.GetClient("IHierarchyService.EndPoint");
        }

        [HttpGet]
        [Route("api/hierarchy/{customerId}")]
        public HierarchyModel Get(long customerId)
        {
            var tree = this.HierarchyService.GetHierarchyTree(customerId);

            return Mapper.Map<HierarchyDto, HierarchyModel>(tree);
        }

        public void Delete(long hierarchyId)
        {
            this.HierarchyService.DeleteHierarchy(hierarchyId, false);            
        }

        public HierarchyModel Post([FromBody]HierarchyModel hierarchy)
        {
            var dto = Mapper.Map<HierarchyModel, HierarchyDto>(hierarchy);

            dto = this.HierarchyService.CreateHierarchy(dto);

            return Mapper.Map<HierarchyDto, HierarchyModel>(dto);
        }

        public void Put([FromBody]HierarchyModel hierarchy)
        {
            var dto = Mapper.Map<HierarchyModel, HierarchyDto>(hierarchy);

            this.HierarchyService.UpdateHierarchy(dto);
        }
    }
}
