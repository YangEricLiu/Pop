using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Common.ClientBehaviors
{
    public class CustomHeaderBehaviorElement : BehaviorExtensionElement
    {
        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <remarks>Return <see cref="CustomHeaderEndpointBehavior" />.</remarks>
        public override Type BehaviorType
        {
            get
            {
                return typeof(CustomHeaderEndpointBehavior);
            }
        }

        /// <summary>
        /// Creates <see cref="CustomHeaderEndpointBehavior" />.
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new CustomHeaderEndpointBehavior();
        }
    }
}