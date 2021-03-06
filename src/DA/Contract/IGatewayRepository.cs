﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.Contract
{
    public interface IGatewayRepository : IRepository<Gateway, long>
    {
        Gateway[] GetByCustomerId(long customerId);

        Gateway[] GetByHierarchyId(long hierarchyId);

        void DeleteGatewayByHierarchyId(IUnitOfWork unitOfWork, long hierarchyId);

        Gateway GetByName(string name);

        Gateway GetByUniqueId(string uniqueId);
    }
}
