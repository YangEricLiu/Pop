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
    public class BusinessLogicException : REMException
    {
        public BusinessLogicException(Layer layer, Module module, int detailCode)
            : base(ErrorType.Logic, layer, module, detailCode)
        {
        }

        public BusinessLogicException(Layer layer, Module module, int detailCode, REMExceptionDetail innerException)
            : base(ErrorType.Logic, layer, module, detailCode, innerException)
        {
        }

        public BusinessLogicException(Layer layer, Module module, int detailCode, string[] messages)
            : base(ErrorType.Logic, layer, module, detailCode, messages)
        {
        }

        public BusinessLogicException(Layer layer, Module module, int detailCode, string[] messages, REMExceptionDetail innerException)
            : base(ErrorType.Logic, layer, module, detailCode, messages)
        {
        }
    }
}