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

        TEntity Add(IUnitOfWork unitOfWork, TEntity entity);

        void Update(TEntity entity);

        void Update(IUnitOfWork unitOfWork, TEntity entity);

        void Delete(TIdType id);

        void Delete(IUnitOfWork unitOfWork, TIdType id);

        TEntity GetOne(ISingleResultQuery<TEntity> singleResultQuery);

        IEnumerable<TEntity> GetMany(IMultiResultQuery<TEntity> multiResultQuery);

        IPaged<TEntity> GetPaged(IPagedResultQuery<TEntity> pagedResultQuery);

        void AddMany(IList<TEntity> entities);

        void UpdateMany(IList<TEntity> entities);
    }
}
