using System;
using System.ServiceModel.Configuration;

namespace SE.DSP.Foundation.Web
{
    public class RemErrorBehaviorExtensionElement : BehaviorExtensionElement
    {
        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <remarks>Return <see cref="CustomHeaderEndpointBehavior" />.</remarks>
        public override Type BehaviorType
        {
            get
            {
                return typeof(RemErrorEndpointBehavior);
            }
        }

        /// <summary>
        /// Creates <see cref="RemErrorEndpointBehavior" />.
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new RemErrorEndpointBehavior();
        }
    }
}