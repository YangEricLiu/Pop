namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    /// <summary>
    /// The base DA API abstract class.
    /// </summary>
    public abstract class DAAPIBase
    {
        /// <summary>
        /// Instantiates an instance of the DAAPIBase class and invoke the <see cref="DAAPIBase.RegisterType()"/> method.
        /// </summary>
        protected DAAPIBase()
        {
            this.RegisterType();
        }

        /// <summary>
        /// Register types those are needed by this DA API into IoC container.
        /// </summary>
        protected abstract void RegisterType();
    }
}