﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class BuildingLocation
    {
        public BuildingLocation()
        {
        }

        public BuildingLocation(long id, decimal? latitude, decimal? longitude, string province, string updateUser)
        {
            this.BuildingId = id;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = updateUser;
            this.Province = province;
        }

        public long BuildingId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateTime { get; set; }

        public long? Version { get; set; }

        public string Province { get; set; }
    }
}
