using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SE.DSP.Foundation.Infrastructure.ActionExtension
{
    public abstract class ActionExtensionBase
    {
        public abstract void Run(IActionContext actionContext);
    }
}
