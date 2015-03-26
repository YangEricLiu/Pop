using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class HierarchyAdministratorDto
    {
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public long HierarchyId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Telephone { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
}
