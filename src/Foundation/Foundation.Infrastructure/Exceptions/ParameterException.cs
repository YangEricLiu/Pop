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
    public class ParameterException : REMException
    {
        public ParameterException(Layer layer, Module module, int detailCode)
            : base(ErrorType.Parameter, layer, module, detailCode)
        {
        }

        public ParameterException(Layer layer, Module module, int detailCode, REMExceptionDetail innerException)
            : base(ErrorType.Parameter, layer, module, detailCode, innerException)
        {
        }

        public ParameterException(Layer layer, Module module, int detailCode, string[] messages)
            : base(ErrorType.Parameter, layer, module, detailCode, messages)
        {
        }

        public ParameterException(Layer layer, Module module, int detailCode, string[] messages, REMExceptionDetail innerException)
            : base(ErrorType.Parameter, layer, module, detailCode, messages, innerException)
        {
        }
    }
}