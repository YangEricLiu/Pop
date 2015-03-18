using System;
using System.ServiceModel.Configuration;

namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// Represents a configuration element that enable the user to customize app service api client endpoint behaviors.
    /// </summary>
    /// <remarks>This class is for internal use only and is not intended to be used directly from your code.</remarks>
    public class CustomHeaderBehaviorExtensionElement : BehaviorExtensionElement
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