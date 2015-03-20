using SE.DSP.Foundation.DataAccess.CustomQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.DataAccess
{
    public interface IRepository<TEntity, TIdType>
    {
        TEntity GetById(TIdType id);

        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TIdType id);

        TEntity GetOne(ISingleResultQuery<TEntity> singleResultQuery);

        IEnumerable<TEntity> GetMany(IMultiResultQuery<TEntity> multiResultQuery);

        IPaged<TEntity> GetPaged(IPagedResultQuery<TEntity> pagedResultQuery);

        void AddMany(IList<TEntity> entities);

        void UpdateMany(IList<TEntity> entities);
    }
}
