using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.DataAccess.CustomQueries
{
    public interface IPagedResultQuery<TEntity>
    {
        Database Db { get; set; }

        IPaged<TEntity> Execute();
    }
}
