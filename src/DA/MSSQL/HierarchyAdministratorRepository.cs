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
    public class HierarchyAdministratorRepository : Repository<HierarchyAdministrator, long>, IHierarchyAdministratorRepository
    {
        public override HierarchyAdministrator GetById(long id)
        {
            var result = this.Db.SingleOrDefault<HierarchyAdministrator>("where Id=@0", id);

            return result;
        }

        public HierarchyAdministrator[] GetByHierarchyId(long hierarchyId)
        {
            var result = this.Db.Query<HierarchyAdministrator>("where hierarchyId = @0", hierarchyId);

            return result.ToArray();
        }

        public override HierarchyAdministrator Add(HierarchyAdministrator entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(HierarchyAdministrator entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override HierarchyAdministrator Add(IUnitOfWork unitOfWork, HierarchyAdministrator entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("HierarchyAdministrator", "Id", entity);

            entity.Id = (long)id;

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, HierarchyAdministrator entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("HierarchyAdministrator", "Id", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<HierarchyAdministrator>(id);
        }

        public void DeleteAdministratorByHierarchyId(IUnitOfWork unitOfWork, long hierarchyId)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<HierarchyAdministrator>("where hierarchyId = @0", hierarchyId);
        }
    }
}
