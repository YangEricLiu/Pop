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
    public class UserCustomerRepository : Repository<UserCustomer, long>, IUserCustomerRepository
    {
        public override UserCustomer GetById(long id)
        {
            var result = this.Db.SingleOrDefault<UserCustomer>("where Id=@0", id);

            return result;
        }

        public override UserCustomer Add(UserCustomer entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(UserCustomer entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override UserCustomer Add(IUnitOfWork unitOfWork, UserCustomer entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("UserCustomer", "Id", entity);

            entity.HierarchyId = (long)id;

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, UserCustomer entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("UserCustomer", "Id", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<UserCustomer>(id);
        }

        public UserCustomer[] GetUserCustomersByUserId(long userId)
        {
            var result = this.Db.Query<UserCustomer>("where UserId = @0", userId);

            return result.ToArray();
        }

        public void DeleteByUserId(IUnitOfWork unitOfWork, long userId)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<UserCustomer>("where UserId = @0", userId);
        }
    }
}
