using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    public interface IPersistor<T> where T : EntityBase, new()
    {
        long Create(T entity);
        void Update(T entity);
        void Delete(long id);
        T RetrieveById(long id);
    }
}
