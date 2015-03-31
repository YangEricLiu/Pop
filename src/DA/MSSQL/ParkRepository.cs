using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SE.DSP.Foundation.DataAccess;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;
using SE.DSP.Pop.Entity.Enumeration;

namespace SE.DSP.Pop.MSSQL
{
    public class ParkRepository : Repository<Park, long>, IParkRepository
    {
        public override Park GetById(long id)
        {
            var result = this.Db.SingleOrDefault<Park>("where HierarchyId=@0", id);

            return result;
        }

        public override Park Add(Park entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(Park entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override Park Add(IUnitOfWork unitOfWork, Park entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("Park", "HierarchyId", false, entity);

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, Park entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("Park", "HierarchyId", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<Park>("where HierarchyId = @0", id);
        }
    }
}
