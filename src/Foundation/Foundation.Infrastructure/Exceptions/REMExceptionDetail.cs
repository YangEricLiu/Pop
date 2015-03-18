/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: TimeRange.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Time range
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Constant;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Configuration;

namespace SE.DSP.Foundation.Infrastructure.Utils.Exceptions
{
    /// <summary>
    /// Error for REM.
    /// </summary>
    public class REMExceptionDetail
    {
        public ErrorType ErrorType { get; set; }
        public int ServerId { get; set; }
        public Layer Layer { get; set; }
        public Module Module { get; set; }
        public int DetailCode { get; set; }
        public string[] ErrorMessages { get; set; }
        public REMExceptionDetail InnerException { get; set; }

        public string ErrorCode
        {
            get
            {
                return Convert.ToInt32(this.ErrorType).ToString("00") + this.ServerId.ToString("0000")
                     + Convert.ToInt32(this.Layer).ToString("0") + Convert.ToInt32(this.Module).ToString("00") + this.DetailCode.ToString("000");
            }
        }

        /// <summary>
        /// Constructor for serialization.
        /// </summary>
        public REMExceptionDetail()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="layer"></param>
        /// <param name="module"></param>
        /// <param name="detailCode"></param>
        public REMExceptionDetail(ErrorType errorType, Layer layer, Module module, int detailCode)
        {
            //this.RequestId = ServiceContextConstant.GetRequestId();

            this.ErrorType = errorType;
            this.ServerId = Convert.ToInt32(ConfigHelper.Get(ConfigurationKey.SERVERID));
            this.Layer = layer;
            this.Module = module;
            this.DetailCode = detailCode;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="layer"></param>
        /// <param name="module"></param>
        /// <param name="detailCode"></param>
        public REMExceptionDetail(ErrorType errorType, Layer layer, Module module, int detailCode, REMExceptionDetail innerException)
            : this(errorType, layer, module, detailCode)
        {
            this.InnerException = innerException;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="layer"></param>
        /// <param name="module"></param>
        /// <param name="errorDetailCode"></param>
        /// <param name="message"><param>
        public REMExceptionDetail(ErrorType errorType, Layer layer, Module module, int detailCode, string[] messages)
            : this(errorType, layer, module, detailCode)
        {
            this.ErrorMessages = messages;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="layer"></param>
        /// <param name="module"></param>
        /// <param name="detailCode"></param>
        /// <param name="messages"></param>
        /// <param name="innerException"></param>
        public REMExceptionDetail(ErrorType errorType, Layer layer, Module module, int detailCode, string[] messages, REMExceptionDetail innerException)
            : this(errorType, layer, module, detailCode, messages)
        {
            this.InnerException = innerException;
        }
    }
}