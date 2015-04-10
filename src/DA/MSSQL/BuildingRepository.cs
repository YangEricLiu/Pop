using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SE.DSP.Foundation.DataAccess;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.MSSQL
{
    public class BuildingRepository : Repository<Building, long>, IBuildingRepository
    {
        public override Building GetById(long id)
        {
            var result = this.Db.SingleOrDefault<Building>("where HierarchyId=@0", id);

            return result;
        }

        public override Building Add(Building entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(Building entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override Building Add(IUnitOfWork unitOfWork, Building entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("Building", "HierarchyId", false, entity);

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, Building entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("Building", "HierarchyId", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<Building>("where HierarchyId = @0", id);
        }
    }
}
