/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: TimeRange.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Time range
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Enumerations;
namespace SE.DSP.Foundation.Infrastructure.Utils.Exceptions
{
    public class DataAuthorizationException : REMException
    {
        public DataAuthorizationException(Layer layer, Module module, int detailCode)
            : base(ErrorType.Authorization, layer, module, detailCode)
        {
        }

        public DataAuthorizationException(Layer layer, Module module, int detailCode, REMExceptionDetail innerException)
            : base(ErrorType.Authorization, layer, module, detailCode, innerException)
        {
        }

        public DataAuthorizationException(Layer layer, Module module, int detailCode, string[] messages)
            : base(ErrorType.Authorization, layer, module, detailCode, messages)
        {
        }

        public DataAuthorizationException(Layer layer, Module module, int detailCode, string[] messages, REMExceptionDetail innerException)
            : base(ErrorType.Authorization, layer, module, detailCode, messages, innerException)
        {
        }

    }
}