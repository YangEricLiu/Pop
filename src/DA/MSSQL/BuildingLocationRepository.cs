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
    public class BuildingLocationRepository : Repository<BuildingLocation, long>, IBuildingLocationRepository
    {
        public override BuildingLocation GetById(long id)
        {
            var result = this.Db.SingleOrDefault<BuildingLocation>("where BuildingId=@0", id);

            return result;
        }

        public override BuildingLocation Add(BuildingLocation entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(BuildingLocation entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override BuildingLocation Add(IUnitOfWork unitOfWork, BuildingLocation entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("BuildingLocation", "BuildingId", false, entity);

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, BuildingLocation entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("BuildingLocation", "BuildingId", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<BuildingLocation>("where BuildingId = @0", id);
        }
    }
}
