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

        public override Entity.Hierarchy GetById(long id)
        {
            var result = this.Db.SingleOrDefault<Hierarchy>(id);

            return result;
        }

        public override Entity.Hierarchy Add(Hierarchy entity)
        {
            var id = this.Db.Insert("Hierarchy", "Id", entity);

            entity.Id = (long)id;

            return entity;
        }

        public override void Update(Hierarchy entity)
        {
            //this.Db.Update()
        }

        public override void Delete(long id)
        {
            this.Db.Delete<Hierarchy>(id);
        }
    }
}
