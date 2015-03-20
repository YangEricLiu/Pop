using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.DataAccess.CustomQueries
{
    public interface IPaged<Entity>
    {
        long CurrentPage { get; }
        long TotalPages { get; }
        long TotalItems { get; }
        long ItemsPerPage { get; }
        IEnumerable<Entity> Items { get; }
    }
}
