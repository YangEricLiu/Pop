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
    public class LogoRepository : Repository<Logo, long>, ILogoRepository
    {
        public override Logo GetById(long id)
        {
            var result = this.Db.SingleOrDefault<Logo>("where Id=@0", id);

            return result;
        }
 
        public override Logo Add(Logo entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(Logo entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override Logo Add(IUnitOfWork unitOfWork, Logo entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("Logo", "Id", entity);

            entity.Id = (long)id;

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, Logo entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<Logo>(id);
        }

        public Logo[] GetLogosByHierarchyIds(long[] hierarchyIds)
        {
            string sql = @"SELECT Id, HierarchyId FROM Logo WHERE HierarchyId in (@0)";

            var result = this.Db.Query<Logo>(sql, hierarchyIds).ToArray();

            return result;
        }

        public void DeleteByHierarchyId(IUnitOfWork unitOfWork, long hierarchyId)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<Logo>("where hierarchyId = @0", hierarchyId);
        }
    }
}
