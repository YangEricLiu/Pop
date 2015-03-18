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
    public class ConcurrentException : REMException
    {
        public ConcurrentException(Layer layer, Module module, int detailCode)
            : base(ErrorType.Concurrent, layer, module, detailCode)
        {
        }

        public ConcurrentException(Layer layer, Module module, int detailCode, REMExceptionDetail innerException)
            : base(ErrorType.Concurrent, layer, module, detailCode, innerException)
        {
        }

        public ConcurrentException(Layer layer, Module module, int detailCode, string[] messages)
            : base(ErrorType.Concurrent, layer, module, detailCode, messages)
        {
        }

        public ConcurrentException(Layer layer, Module module, int detailCode, string[] messages, REMExceptionDetail innerException)
            : base(ErrorType.Concurrent, layer, module, detailCode, messages, innerException)
        {
        }
    }
}