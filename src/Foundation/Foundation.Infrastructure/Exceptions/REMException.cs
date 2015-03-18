/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: TimeRange.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Time range
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SE.DSP.Foundation.Infrastructure.Utils.Exceptions
{
    public class REMException : FaultException<REMExceptionDetail>
    {
        public REMException(ErrorType errorType, Layer layer, Module module, int code)
            : base(new REMExceptionDetail(errorType, layer, module, code), string.Empty)
        {
        }

        public REMException(ErrorType errorType, Layer layer, Module module, int code, REMExceptionDetail innerException)
            : base(new REMExceptionDetail(errorType, layer, module, code, innerException), string.Empty)
        {
        }

        public REMException(ErrorType errorType, Layer layer, Module module, int code, string[] messages)
            : base(new REMExceptionDetail(errorType, layer, module, code, messages), string.Empty)
        {
        }

        public REMException(ErrorType errorType, Layer layer, Module module, int code, string[] messages, REMExceptionDetail innerException)
            : base(new REMExceptionDetail(errorType, layer, module, code, messages, innerException), string.Empty)
        {
        }

        public string Code
        {
            get
            {
                return Convert.ToInt32(this.Detail.ErrorType).ToString("00") + this.Detail.ServerId.ToString("0000")
                   + Convert.ToInt32(this.Detail.Layer).ToString("0") + Convert.ToInt32(this.Detail.Module).ToString("00") + this.Detail.DetailCode.ToString("000");
            }
        }

        public  string[] Messages
        {
            get
            {
                return this.Detail.ErrorMessages;
            }
        }
    }
}