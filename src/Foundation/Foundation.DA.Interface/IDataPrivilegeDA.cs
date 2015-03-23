using SE.DSP.Foundation.Infrastructure.BE.Entities; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.DA.Interface
{
    public interface IDataPrivilegeDA
    {
        void CreateDataPrivileges(DataPrivilegeEntity[] entities);
        void DeleteDataPrivileges(DataPrivilegeFilter filter);
        DataPrivilegeEntity[] RetrieveDataPrivileges(DataPrivilegeFilter filter);

        long[] RetrieveCustomerIdsByPrivilegeItem(long[] itemIds);


        void UpdateDataprivileges(long[] privileges);
    }
}
