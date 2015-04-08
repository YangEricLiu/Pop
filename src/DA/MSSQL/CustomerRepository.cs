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
    public class CustomerRepository : Repository<Customer, long>, ICustomerRepository
    {
        public override Customer GetById(long id)
        {
            var result = this.Db.SingleOrDefault<Customer>("where HierarchyId=@0", id);

            return result;
        }

        public override Customer Add(Customer entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(Customer entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override Customer Add(IUnitOfWork unitOfWork, Customer entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("Customer", "HierarchyId", false, entity);

            entity.HierarchyId = (long)id;

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, Customer entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("Customer", "HierarchyId", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<Customer>("where HierarchyId = @0", id);
        }

        public Customer[] GetByIds(long[] ids)
        {
            var result = this.Db.Query<Customer>("where HierarchyId in (@0)", ids);

            return result.ToArray();
        }

        public Customer[] GetBySpId(long spId)
        {
            var result = this.Db.Query<Customer>("where spid = @0", spId);

            return result.ToArray();
        }

        public Customer GetByCode(string customerCode)
        {
            var sql = @"select * from Customer C
                        inner join Hierarchy H on H.Id = C.HierarchyId
                        where H.Code = @0";

            var result = this.Db.SingleOrDefault<Customer>(sql, customerCode);

            return result;
        }
    }
}
