

using SE.DSP.Foundation.Infrastructure.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class ServiceProviderEntity : EntityBase
    {
        public String UserName { get; set; }
        public String Address { get; set; }
        public String Telephone { get; set; }
        public String Email { get; set; }
        public string Domain { get; set; }
        public DateTime? StartDate { get; set; }

        public DeployStatus DeployStatus { get; set; }


        public bool CalcStatus { set; get; }
    }
    public enum DeployStatus
    {
        Processing = 0,
        Finished = 1
    }
}
