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
    public class HierarchyController : ApiController
    {
        private IHierarchyService HierarchyServiceProxy;

        public HierarchyController()
        {
            HierarchyServiceProxy = ServiceProxy<IHierarchyService>.GetClient("IHierarchyService.EndPoint");
        }

        public HierarchyModel Get(long customerId)
        {
            var tree = this.HierarchyServiceProxy.GetHierarchyTree(customerId);

            return Mapper.Map<HierarchyDto, HierarchyModel>(tree);
        }

        public void Delete(long hierarchyId)
        {
            this.HierarchyServiceProxy.DeleteHierarchy(hierarchyId, false);            
        }

        public HierarchyModel Post([FromBody]HierarchyModel hierarchy)
        {
            var dto = Mapper.Map<HierarchyModel, HierarchyDto>(hierarchy);

            dto = this.HierarchyServiceProxy.CreateHierarchy(dto);

            return Mapper.Map<HierarchyDto, HierarchyModel>(dto);
        }

        public void Put([FromBody]HierarchyModel hierarchy)
        {
            var dto = Mapper.Map<HierarchyModel, HierarchyDto>(hierarchy);

            this.HierarchyServiceProxy.UpdateHierarchy(dto);
        }
    }
}
