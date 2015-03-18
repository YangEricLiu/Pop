/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: LogBehaviorExtensionElement.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : The extension for log
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/


using System;
using System.ServiceModel.Configuration;

namespace SE.DSP.Foundation.Infrastructure.Interception
{
    public class LogBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(LogServiceBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new LogServiceBehavior();
        }
    }
}