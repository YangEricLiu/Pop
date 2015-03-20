using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        Database Db { get; }
        void Commit();
    }
}
