using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SE.DSP.Foundation.Infrastructure;

using Microsoft.Practices.Unity;

using SE.DSP.Foundation.Infrastructure.Utils;

namespace SE.DSP.Foundation.DA.API
{
    using SE.DSP.Foundation.Infrastructure.BaseClass;
    using SE.DSP.Foundation.Infrastructure.BE.Entities;
    using SE.DSP.Foundation.DA.Interface;
   
    public class DataPrivilegeAPI:DAAPIBase
    {
        #region DI

        private IDataPrivilegeDA _dataPrivilegeDa;
        private IDataPrivilegeDA DataPrivilegeDA
        {
            get
            {
                return this._dataPrivilegeDa ?? (this._dataPrivilegeDa = IocHelper.Container.Resolve<IDataPrivilegeDA>());
            }
        }

        /// <summary>
        /// Register types those are needed by calendar data access api into IoC container.
        /// </summary>
        protected override void RegisterType()
        {
            IocHelper.Container.RegisterInstanceSingleton((IDataPrivilegeDA)DAFactory.CreateDA(typeof(IDataPrivilegeDA)));
        }
        #endregion

        public void CreateDataPrivileges(DataPrivilegeEntity[] entities)
        {
            DataPrivilegeDA.CreateDataPrivileges(entities);
        }
        public void DeleteDataPrivileges(DataPrivilegeFilter filter)
        {
            DataPrivilegeDA.DeleteDataPrivileges(filter);
        }


        public void UpdateDataprivileges(long[] privileges)
        {
            DataPrivilegeDA.UpdateDataprivileges(privileges);
        }
        public DataPrivilegeEntity[] RetrieveDataPrivileges(DataPrivilegeFilter filter)
        {
            return DataPrivilegeDA.RetrieveDataPrivileges(filter);
        }

        public long[] RetrieveCustomerIdsByPrivilegeItem(long[] itemIds)
        {
            return DataPrivilegeDA.RetrieveCustomerIdsByPrivilegeItem(itemIds);
        }

    

    }
}
