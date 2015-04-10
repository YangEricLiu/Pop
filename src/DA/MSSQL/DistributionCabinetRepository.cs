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
    public class DistributionCabinetRepository : Repository<DistributionCabinet, long>, IDistributionCabinetRepository
    {
        public override DistributionCabinet GetById(long id)
        {
            var result = this.Db.SingleOrDefault<DistributionCabinet>("where HierarchyId=@0", id);

            return result;
        }

        public override DistributionCabinet Add(DistributionCabinet entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(DistributionCabinet entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override DistributionCabinet Add(IUnitOfWork unitOfWork, DistributionCabinet entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("DistributionCabinet", "HierarchyId", false, entity);

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, DistributionCabinet entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("DistributionCabinet", "HierarchyId", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<DistributionCabinet>("where HierarchyId = @0", id);
        }
    }
}
