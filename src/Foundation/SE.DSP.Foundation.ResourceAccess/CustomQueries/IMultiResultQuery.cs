using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.DataAccess.CustomQueries
{
    public interface IMultiResultQuery<TEntity>
    {
        Database Db { get; set; }

        IEnumerable<TEntity> Execute();
    }
}
