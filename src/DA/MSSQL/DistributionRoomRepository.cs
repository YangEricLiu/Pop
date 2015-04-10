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
    public class DistributionRoomRepository : Repository<DistributionRoom, long>, IDistributionRoomRepository
    {
        public override DistributionRoom GetById(long id)
        {
            var result = this.Db.SingleOrDefault<DistributionRoom>("where HierarchyId=@0", id);

            return result;
        }

        public override DistributionRoom Add(DistributionRoom entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(DistributionRoom entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override DistributionRoom Add(IUnitOfWork unitOfWork, DistributionRoom entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("DistributionRoom", "HierarchyId", false, entity);

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, DistributionRoom entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("DistributionRoom", "HierarchyId", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<DistributionRoom>("where HierarchyId = @0", id);
        }
    }
}
