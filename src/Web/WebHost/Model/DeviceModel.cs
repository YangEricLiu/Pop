namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class DeviceModel
    {
        public long? HierarchyId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public long ParentHierarchyId { get; set; }

        public string Factory { get; set; }

        public string Description { get; set; }

        public long? GatewayId { get; set; }

        public LogoModel Picture { get; set; }
    }
}