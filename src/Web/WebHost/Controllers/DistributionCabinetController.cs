﻿using System.Web.Http;
using AutoMapper;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    public class DistributionCabinetController : ApiController
    {
        private readonly IHierarchyService hierarchyService;

        public DistributionCabinetController()
        {
            this.hierarchyService = ServiceProxy<IHierarchyService>.GetClient("IHierarchyService.EndPoint");
        }

        [HttpGet]
        [Route("api/distributionCabinet/{hierarchyId}")]
        public DistributionCabinetModel Get(long hierarchyId)
        {
            var tree = this.hierarchyService.GetDistributionCabinetById(hierarchyId);

            return Mapper.Map<DistributionCabinetDto, DistributionCabinetModel>(tree);
        }

        [HttpPost]
        [Route("api/distributionCabinet/delete/{hierarchyId}")]
        public void Delete(long hierarchyId)
        {
            this.hierarchyService.DeleteDistributionCabinet(hierarchyId);            
        }

        [HttpPost]
        [Route("api/distributionCabinet/create")]
        public DistributionCabinetModel Create([FromBody]DistributionCabinetModel DistributionCabinet)
        {
            var dto = Mapper.Map<DistributionCabinetModel, DistributionCabinetDto>(DistributionCabinet);

            dto = this.hierarchyService.CreateDistributionCabinet(dto);

            return Mapper.Map<DistributionCabinetDto, DistributionCabinetModel>(dto);
        }

        [HttpPost]
        [Route("api/distributionCabinet/update")]
        public DistributionCabinetModel Update([FromBody]DistributionCabinetModel DistributionCabinet)
        {
            var dto = Mapper.Map<DistributionCabinetModel, DistributionCabinetDto>(DistributionCabinet);

            var result = this.hierarchyService.UpdateDistributionCabinet(dto);

            return Mapper.Map<DistributionCabinetDto, DistributionCabinetModel>(result);
        }
    }
}
