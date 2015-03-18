using System;
using System.Web;

namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// The http module for web service api wrapper.
    /// </summary>
    /// <remarks>
    /// <para>This http module will generate request GUID for current request.</para>
    /// <para>This class is for internal use only and is not intended to be used directly from your code.</para>
    /// </remarks>
    public class ServiceWrapperHttpModule : IHttpModule
    {
        /// <summary>
        /// Initializes this module to subscribe the <see cref="HttpApplication.BeginRequest" /> event.
        /// </summary>
        /// <param name="context">An HttpApplication that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += this.Application_BeginRequest;
        }

        /// <summary>
        /// Create request GUID for current request when the http application begins requesting.
        /// </summary>
        /// <param name="source">The source of the event. </param>
        /// <param name="e">An EventArgs that contains no event data. </param>
        private void Application_BeginRequest(Object source, EventArgs e)
        {
            ServiceWrapperContext.RequestId = Guid.NewGuid();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public void Dispose()
        {
        }
    }
}