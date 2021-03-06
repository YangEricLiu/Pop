﻿using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class ParkDto : BaseHierarchyDto
    { 
        [DataMember]
        public decimal? FloorSpace { get; set; }

        [DataMember]
        public decimal? BuildingArea { get; set; }

        [DataMember]
        public LogoDto Logo { get; set; }

        [DataMember]
        public BuildingLocationDto Location { get; set; }

        [DataMember]
        public HierarchyAdministratorDto[] Administrators { get; set; }

        [DataMember]
        public GatewayDto[] Gateways { get; set; }
    }
}
