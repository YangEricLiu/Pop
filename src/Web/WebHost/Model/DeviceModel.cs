namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class DeviceModel : BaseHierarchyModel
    { 
        public string Factory { get; set; }

        public string Description { get; set; }

        public long? GatewayId { get; set; }

        public LogoModel Logo { get; set; }

        public SceneLogModel[] SceneLogs { get; set; }

        public ScenePictureModel[] ScenePicture { get; set; }
    }
}