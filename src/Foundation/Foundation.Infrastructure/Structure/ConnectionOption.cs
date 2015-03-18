using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.Structure
{
    public class ConnectionOption
    {
        public REMDbType? DbType { get; set; }
        public String ConnectionString { get; set; }
    }

    public enum REMDbType
    {
        Metadata,
        ServiceProvider,
        All,
    }
}
