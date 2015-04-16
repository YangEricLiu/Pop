using System;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class SceneLogModel
    {
        public long? Id { get; set; }

        public long HierarchyId { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateUser { get; set; }
    }
}
