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
    public class HierarchyRepository : Repository<Hierarchy, long>, IHierarchyRepository
    {
        public override Hierarchy GetById(long id)
        {
            var result = this.Db.SingleOrDefault<Hierarchy>(id);

            return result;
        }

        public Hierarchy[] GetByIds(long[] ids)
        {
            var hierarchies = this.Db.Fetch<Hierarchy>("where Id in (@0)", ids);

            return hierarchies.ToArray();
        }

        public Hierarchy[] GetByParentId(long parentId)
        {
            var children = this.Db.Fetch<Hierarchy>("where ParentId=@0", parentId);

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

        public long RetrieveSiblingHierarchyCountByCodeUnderParentCustomer(long hierarchyId, string hierarchyCode, long customerId)
        {
            string sql = @"SELECT COUNT(0) FROM Hierarchy WHERE Id<>@0 AND Code=@1 AND CustomerId=@2";

            var count = this.Db.ExecuteScalar<long>(sql, hierarchyId, hierarchyCode, customerId);

            return count;
        }

        public long RetrieveChildHierarchyCountByCodeUnderParentCustomer(string hierarchyCode, long customerId)
        {
            string sql = @"SELECT COUNT(*) FROM Hierarchy WHERE Code=@0 AND CustomerId=@1";

            var count = this.Db.ExecuteScalar<long>(sql, hierarchyCode, customerId);

            return count;
        }

        public long RetrieveAncestorAndSelfOrganizationCount(long organizationId)
        {
            string sql = @"WITH Child
                                    AS 
                                    (
                                        SELECT Id, ParentId FROM Hierarchy  WHERE Id=@0 AND Type=@1
                                        UNION ALL
                                        SELECT Parent.Id, Parent.ParentId FROM Hierarchy Parent INNER JOIN Child ON Parent.Id = Child.ParentId WHERE Parent.Type=@1
                                    )
        
                           SELECT COUNT(*) FROM Child";

            var count = this.Db.ExecuteScalar<long>(sql, organizationId, Convert.ToInt32(HierarchyType.Organization));

            return count;
        }

        public long RetrieveSiblingHierarchyCountByNameUnderParentCustomer(long hierarchyId, string hierarchyName, long parentCustomerId)
        {
            string sql = @"SELECT COUNT(*) FROM Hierarchy WHERE Id<>@0 AND Name=@1 AND CustomerId=@2 AND ParentId IS NULL";

            var count = this.Db.ExecuteScalar<long>(sql, hierarchyId, hierarchyName, parentCustomerId);

            return count;
        }

        public long RetrieveChildHierarchyCountByNameUnderParentCustomer(string hierarchyName, long parentCustomerId)
        {
            string sql = @"SELECT COUNT(*) FROM Hierarchy WHERE Name=@0 AND CustomerId=@1 AND ParentId IS NULL";

            var count = this.Db.ExecuteScalar<long>(sql, hierarchyName, parentCustomerId);

            return count;
        }

        public long RetrieveSiblingHierarchyCountByNameUnderParentHierarchy(long hierarchyId, string hierarchyName, long parentHierarchyId)
        {
            string sql = @"SELECT COUNT(*) FROM Hierarchy WHERE Id<>@0 AND Name=@1 AND ParentId=@2";

            var count = this.Db.ExecuteScalar<long>(sql, hierarchyId, hierarchyName, parentHierarchyId);

            return count;
        }

        public long RetrieveChildHierarchyCountByNameUnderParentHierarchy(string hierarchyName, long parentHierarchyId)
        {
            string sql = @"SELECT COUNT(*) FROM Hierarchy WHERE Name=@0 AND ParentId=@1";

            var count = this.Db.ExecuteScalar<long>(sql, hierarchyName, parentHierarchyId);

            return count;
        }
    }
}
