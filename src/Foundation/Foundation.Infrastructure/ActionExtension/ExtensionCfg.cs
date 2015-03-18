using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SE.DSP.Foundation.Infrastructure.ActionExtension
{
    public class ActionCfg
    {
          [XmlAttribute]
        public int Order { set; get; }
          [XmlAttribute]
        public string ActionType { set; get; }
    }


    public class ActionExtensionCfg
    {
        [XmlElement("ActionCfg")]
        public List<ActionCfg> ActionCfgs { set; get; }
         [XmlAttribute]
        public string Path { set; get; }
    }


    public class ActionExtensionCfgs
    {
          [XmlElement("ActionExtensionCfg")]
        public List<ActionExtensionCfg> ActionExtensionCfgCollection { set; get; }
    }
}
