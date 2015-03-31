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
    public class GatewayRepository : Repository<Gateway, long>, IGatewayRepository
    {
        public override Gateway GetById(long id)
        {
            var result = this.Db.SingleOrDefault<Gateway>(id);

            return result;
        }

        public override Gateway Add(Gateway entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(Gateway entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override Gateway Add(IUnitOfWork unitOfWork, Gateway entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("Gateway", "Id", true, entity);

            entity.Id = (long)id;

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, Gateway entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("Gateway", "Id", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<Gateway>(id);
        }

        public Gateway[] GetByCustomerId(long customerId)
        {
            var result = this.Db.Query<Gateway>("where customerid = @0", customerId);

            return result.ToArray();
        }

        public Gateway GetByName(string name) 
        {
            var result = this.Db.Query<Gateway>("where Name = @0", name);

            return result.SingleOrDefault();
        }
    }
}
