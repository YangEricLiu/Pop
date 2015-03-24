using SE.DSP.Foundation.DataAccess;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.MSSQL
{
    public class HierarchyRepository : Repository<Hierarchy, long>, IHierarchyRepository
    {
        public override Hierarchy GetById(long id)
        {
            var result = this.Db.SingleOrDefault<Hierarchy>("select * from [Hierarchy] where Id=@0", id);

            return result;
        }

        public Hierarchy[] GetByParentId(long parentId)
        {
            var sql = "select * from [Hierarchy] where ParentId=@0";
            var children = this.Db.Fetch<Hierarchy>(sql, parentId);

            return children.ToArray();
        }

        public override Hierarchy Add(Hierarchy entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(Hierarchy entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override Hierarchy Add(IUnitOfWork unitOfWork, Hierarchy entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("Hierarchy", "Id", entity);

            entity.Id = (long)id;

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, Hierarchy entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<Hierarchy>(id);
        }
    }
}
