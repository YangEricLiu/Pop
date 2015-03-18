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
    public class InputException : REMException
    {
        public InputException(Layer layer, Module module, int detailCode)
            : base(ErrorType.Input, layer, module, detailCode)
        {
        }

        public InputException(Layer layer, Module module, int detailCode, REMExceptionDetail innerException)
            : base(ErrorType.Input, layer, module, detailCode, innerException)
        {
        }

        public InputException(Layer layer, Module module, int detailCode, string[] messages)
            : base(ErrorType.Input, layer, module, detailCode, messages)
        {
        }

        public InputException(Layer layer, Module module, int detailCode, string[] messages, REMExceptionDetail innerException)
            : base(ErrorType.Input, layer, module, detailCode, messages, innerException)
        {
        }
    }
}