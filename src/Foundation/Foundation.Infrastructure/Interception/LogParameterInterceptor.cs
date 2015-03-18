/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: LogParameterInterceptor.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : For log method prameters
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    public class LogParameterInterceptor : IParameterInspector
    {
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            StringBuilder logMessage = new StringBuilder();

            for (int i = 0; i < outputs.Length; i++)
            {
                logMessage.Append("Out Parameter - ").Append(outputs[i] ?? "Null").AppendLine();
            }

            if (returnValue != null && returnValue.GetType().IsArray)
            {
                Array array = returnValue as Array;

                int arrayLength = array.GetLength(0);

                logMessage.AppendLine().Append("Return Value - ");

                for (int j = 0; j < arrayLength; j++)
                {
                    logMessage.Append("[").Append(array.GetValue(j) ?? "Null").Append("]");
                }
            }
            else
            {
                logMessage.Append("Return Value - ").Append(returnValue ?? "Null").AppendLine();
            }

            logMessage.Append("End of ").Append(operationName);

            LogHelper.LogDebug(logMessage.ToString());
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            StringBuilder logMessage = new StringBuilder("Beginning of ").Append(operationName);

            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] != null && inputs[i].GetType().IsArray)
                {
                    Array array = inputs[i] as Array;

                    int arrayLength = array.GetLength(0);

                    logMessage.AppendLine() .Append("In Parameter - ");

                    for (int j = 0; j < arrayLength; j++)
                    {
                        logMessage.Append("[").Append(array.GetValue(j) ?? "Null").Append("]");
                    }
                }
                else
                {
                    logMessage.AppendLine().Append("In Parameter - ").Append(inputs[i] ?? "Null");
                }
            }

            LogHelper.LogDebug(logMessage.ToString());

            return null;
        }
    }
}
