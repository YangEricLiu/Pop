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
   public class CustomException: REMException
    {
        public CustomException(Layer layer, Module module, int detailCode)
            : base(ErrorType.Custom, layer, module, detailCode)
        {
        }

        public CustomException(Layer layer, Module module, int detailCode, REMExceptionDetail innerException)
            : base(ErrorType.Custom, layer, module, detailCode, innerException)
        {
        }

        public CustomException(Layer layer, Module module, int detailCode, string[] messages)
            : base(ErrorType.Custom, layer, module, detailCode, messages)
        {
        }

        public CustomException(Layer layer, Module module, int detailCode, string[] messages, REMExceptionDetail innerException)
            : base(ErrorType.Custom, layer, module, detailCode, messages)
        {
        }
    }
}