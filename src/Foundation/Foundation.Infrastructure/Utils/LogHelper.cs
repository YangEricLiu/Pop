/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: LogHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Helper for log
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

//using Aliyun.OpenServices.OpenTableService;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.Infrastructure.Constant;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using System;
using System.Configuration;
using System.ServiceModel;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// The log utility class.
    /// </summary>
    public static class LogHelper
    {
        #region Constructor
        static LogHelper()
        {
            IocHelper.Container.RegisterInstanceSingleton(new LogWriterFactory().Create());
        }
        #endregion

        #region Property
        private static LogWriter _logWriter;

        private static LogWriter LogWriter
        {
            get { return _logWriter ?? (_logWriter = IocHelper.Container.Resolve<LogWriter>()); }
        }
        #endregion

        #region Exception
        /// <summary>
        /// Log exception.
        /// </summary>
        /// <param name="ex">The excption need be logged.</param>
        /// <param name="severity">The severity of this exception, default value is <see cref="LoggingSeverity.Error" />.</param>
        public static void LogException(Exception ex, LoggingSeverity severity = LoggingSeverity.Error)
        {
            string message;

            //if (ex is OtsException)
            //{
            //    //OtsException otsEx = (OtsException)ex;

            //    //message = new StringBuilder(otsEx.Message)
            //    //    .AppendLine()
            //    //    .Append("OTS error code is ")
            //    //    .Append(otsEx.ErrorCode)
            //    //    .Append("; OTS request id is ")
            //    //    .Append(otsEx.RequestId)
            //    //    .Append("; OTS host id is ")
            //    //    .Append(otsEx.HostId)
            //    //    .AppendLine()
            //    //    .Append(ex.StackTrace)
            //    //    .ToString();
            //}
            //else 
            if (ex is FaultException<REMExceptionDetail>) //BL exception
            {
                //log the error code
                message = new StringBuilder((ex as FaultException<REMExceptionDetail>).Detail.ErrorCode).AppendLine().Append(ex.StackTrace).ToString();
            }
            else if (ex is FaultException<ValidationFault>) //validate exception
            {
                //log the error code
                message = new StringBuilder((ex as FaultException<ValidationFault>).Detail.Details[0].Message).AppendLine().Append(ex.StackTrace).ToString();
            }
            else
            {
                message = new StringBuilder(ex.Message).AppendLine().Append(ex.StackTrace).ToString();
            }

            //Log
            WriteLog(message, severity, (Guid?)ex.Data[ServiceContextConstant.REQUESTID]);
        }
        #endregion

        #region Message
        /// <summary>
        /// Log fatal.
        /// </summary>
        /// <param name="message">The fatal message.</param>
        public static void LogFatal(string message)
        {
            WriteLog(message, LoggingSeverity.Fatal);
        }

        /// <summary>
        /// Log error.
        /// </summary>
        /// <param name="message">The error message.</param>
        public static void LogError(string message)
        {
            WriteLog(message, LoggingSeverity.Error);
        }

        /// <summary>
        /// Log warning.
        /// </summary>
        /// <param name="message">The warning message.</param>
        public static void LogWarning(string message)
        {
            WriteLog(message, LoggingSeverity.Warning);
        }

        /// <summary>
        /// Log information.
        /// </summary>
        /// <param name="message">The information message.</param>
        public static void LogInformation(string message)
        {
            WriteLog(message, LoggingSeverity.Information);
        }

        /// <summary>
        /// Log debug.
        /// </summary>
        /// <param name="message">The debug message.</param>
        public static void LogDebug(string message)
        {
            WriteLog(message, LoggingSeverity.Debug);
        }
        #endregion

        #region Common
        /// <summary>
        /// Check whether need logging according to the severity.
        /// </summary>
        /// <param name="severity">The logging severity.</param>
        /// <returns>true if need logging, else false.</returns>
        private static bool DoesNeedLogging(LoggingSeverity severity)
        {
#if DEBUG
            LoggingSeverity loggingSeveritySetting = (LoggingSeverity)Enum.Parse(typeof(LoggingSeverity), ConfigurationManager.AppSettings[ConfigurationKey.LOGGINGSEVERITY]);

            if (loggingSeveritySetting >= severity)
            {
                return true;
            }
            else
            {
                return false;
            }
#else
            if (severity == LoggingSeverity.Fatal || severity == LoggingSeverity.Error)
            {
                return true;
            }
            else
            {
                return false;
            }
#endif
        }

        /// <summary>
        /// Write log to log file.
        /// </summary>
        /// <param name="message">The log message which need be logged.</param>
        /// <param name="severity">The error severity.</param>
        /// <param name="requestId">The reqeust id.</param>
        /// <remarks>The title property of <see cref="LogEntry" /> represents the request id.</remarks>
        private static void WriteLog(string message, LoggingSeverity severity, Guid? requestId = null)
        {
            if (DoesNeedLogging(severity))
            {
                LogEntry logEntry = new LogEntry()
                {
                    TimeStamp = DateTime.UtcNow,
                    Message = new StringBuilder(severity.ToString()).Append(ASCII.SPACE).Append(message).ToString(),
                };

                logEntry.Title = requestId.HasValue ? requestId.Value.ToString() : ServiceContextConstant.GetRequestId().ToString();

                logEntry.Categories.Add("Log");

                LogWriter.Write(logEntry);
            }
        }
        #endregion
    }
}