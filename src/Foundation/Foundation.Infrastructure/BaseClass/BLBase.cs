
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using System;

namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    /// <summary>
    /// The base BL abstract class.
    /// </summary>
    public abstract class BLBase
    {
        /// <summary>
        /// Instantiates an instance of the BLBase class and invoke the <see cref="BLBase.RegisterType()"/> method.
        /// </summary>
        protected BLBase()
        {
            this.RegisterType();
        }

        /// <summary>
        /// Register types those are needed by this BL into IoC container.
        /// </summary>
        protected abstract void RegisterType();

        protected void ThrowDataAuthorizationException()
        {
            throw new DataAuthorizationException(Layer.BL, Module.AccessControl, Convert.ToInt32(ErrorCode.NoDataPrivilege));
        }
        protected static StatusFilter GetStatusFilter()
        {
            return new StatusFilter { ExcludeStatus = true, Statuses = new[] { EntityStatus.Deleted } };
        }
    }
}