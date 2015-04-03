using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class BuildingLocationDto
    { 
        [DataMember]
        public long BuildingId { get; set; }

        [DataMember]
        public decimal? Latitude { get; set; }

        [DataMember]
        public decimal? Longitude { get; set; }

        [DataMember]
        public string Province { get; set; }
    }
}
