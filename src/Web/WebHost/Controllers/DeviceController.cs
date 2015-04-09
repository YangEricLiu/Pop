using System.Web.Http;
using AutoMapper;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    [Authorize]
    public class DeviceController : ApiController
    {
        private readonly IHierarchyService hierarchyService;

        public DeviceController()
        {
            this.hierarchyService = ServiceProxy<IHierarchyService>.GetClient("IHierarchyService.EndPoint");
        }

        [HttpGet]
        [Route("api/device/{hierarchyId}")]
        public DeviceModel Get(long hierarchyId)
        {
            var tree = this.hierarchyService.GetDeviceById(hierarchyId);

            return Mapper.Map<DeviceDto, DeviceModel>(tree);
        }

        [HttpPost]
        [Route("api/device/delete/{hierarchyId}")]
        public void Delete(long hierarchyId)
        {
            this.hierarchyService.DeleteDevice(hierarchyId);            
        }

        [HttpPost]
        [Route("api/device/create")]
        public DeviceModel Create([FromBody]DeviceModel device)
        {
            var dto = Mapper.Map<DeviceModel, DeviceDto>(device);

            dto = this.hierarchyService.CreateDevice(dto);

            return Mapper.Map<DeviceDto, DeviceModel>(dto);
        }

        [HttpPost]
        [Route("api/device/update")]
        public DeviceModel Update([FromBody]DeviceModel device)
        {
            var dto = Mapper.Map<DeviceModel, DeviceDto>(device);

            var result = this.hierarchyService.UpdateDevice(dto);

            return Mapper.Map<DeviceDto, DeviceModel>(result);
        }
    }
}
