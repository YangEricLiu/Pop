using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.ActionExtension
{
    internal class DefaultActionContext : IActionContext
    {
        private Dictionary<string, string> hostDic;//todo param may be  complex type

        private Dictionary<string, object> pipelineDic;
        private Dictionary<string, string> HostDic
        {
            get
            {
                if (hostDic == null)
                {
                    hostDic = new Dictionary<string, string>();
                }
                return hostDic;
            }

        }
        private Dictionary<string, object> PipelineDic
        {
            get
            {
                if (pipelineDic == null)
                {
                    pipelineDic = new Dictionary<string, object>();
                }
                return pipelineDic;
            }
        }

        public void SetHostValue(string key, string value)
        {
            HostDic.Add(key, value);
        }

        public void SetHostValue(Dictionary<string,string> hostDict)
        {
            this.hostDic = hostDict;
        }

        public string GetHostValue(string key)
        {
            string val;
            if (HostDic.TryGetValue(key, out val))
            {
                return val;
            }

            return "";
        }

        public object this[string key]
        {
            get
            {
                object v = null;
                if (this.PipelineDic.TryGetValue(key, out v))
                {
                    return v;
                }

                return null;
            }
            set
            {
                if (value == null) return;
                this.PipelineDic.Add(key, value);
            }
        }

        public DefaultActionContext()
        {
        }


        public bool IsInTransaction
        {
            set;
            get;
        }

        public PositionType PositionType
        {
            set;
            get;
        }
    }
}
