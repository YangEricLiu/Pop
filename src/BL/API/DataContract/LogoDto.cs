using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class LogoDto
    { 
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public long? HierarchyId { get; set; }

        [DataMember]
        public byte[] Logo { get; set; }
    }
}
