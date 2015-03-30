using System.Web.Http;
using AutoMapper;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    [Authorize]
    public class OrganizationController : ApiController
    {
        private readonly IHierarchyService hierarchyService;

        public OrganizationController()
        {
            this.hierarchyService = ServiceProxy<IHierarchyService>.GetClient("IHierarchyService.EndPoint");
        }

        [HttpGet]
        [Route("api/organization/{hierarchyId}")]
        public OrganizationModel Get(long hierarchyId)
        {
            var tree = this.hierarchyService.GetOrganizationById(hierarchyId);

            return Mapper.Map<OrganizationDto, OrganizationModel>(tree);
        }

        [HttpPost]
        [Route("api/organization/delete/{hierarchyId}")]
        public void Delete(long hierarchyId)
        {
            this.hierarchyService.DeleteOrganization(hierarchyId);            
        }

        [HttpPost]
        [Route("api/organization/create")]
        public OrganizationModel Post([FromBody]OrganizationModel organization)
        {
            var dto = Mapper.Map<OrganizationModel, OrganizationDto>(organization);

            dto = this.hierarchyService.CreateOrganization(dto);

            return Mapper.Map<OrganizationDto, OrganizationModel>(dto);
        }

        [HttpPost]
        [Route("api/organization/update")]
        public OrganizationModel Put([FromBody]OrganizationModel organization)
        {
            var dto = Mapper.Map<OrganizationModel, OrganizationDto>(organization);

            var result = this.hierarchyService.UpdateOrganization(dto);

            return Mapper.Map<OrganizationDto, OrganizationModel>(result);
        }
    }
}
