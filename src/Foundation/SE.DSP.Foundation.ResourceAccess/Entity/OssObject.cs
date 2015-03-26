using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.DataAccess.Entity
{
    public class OssObject
    {
        public OssObject(string key, byte[] content)
        {
            this.Key = key;
            this.Content = content;
        }

        public string Key { get; set; }

        public byte[] Content { get; set; }
    }
}
