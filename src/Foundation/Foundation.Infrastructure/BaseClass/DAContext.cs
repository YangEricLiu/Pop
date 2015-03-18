
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    public static class DAContext
    {
        public static Language Language
        {
            get
            {
                if (OperationContext.Current == null)
                    return Language.ZH_CN;
                bool res = OperationContext.Current.IncomingMessageHeaders.Any(temp => temp.Name.Equals(ServiceContextConstant.LANGUAGE));
                if (!res)
                    return Language.ZH_CN;
                else
               return OperationContext.Current.IncomingMessageHeaders.GetHeader<Language>(ServiceContextConstant.LANGUAGE, ServiceContextConstant.HEADERNAMESPACE);
            }
        }
    }
}
