namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class ScenePictureModel
    {
        public long? Id { get; set; }

        public long HierarchyId { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public byte[] Content { get; set; }
    }
}
