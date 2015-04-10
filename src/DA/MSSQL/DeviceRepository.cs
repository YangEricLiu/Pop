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
    public class DeviceRepository : Repository<Device, long>, IDeviceRepository
    {
        public override Device GetById(long id)
        {
            var result = this.Db.SingleOrDefault<Device>("where HierarchyId=@0", id);

            return result;
        }

        public override Device Add(Device entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(Device entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override Device Add(IUnitOfWork unitOfWork, Device entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("Device", "HierarchyId", false, entity);

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, Device entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("Device", "HierarchyId", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<Device>("where HierarchyId = @0", id);
        }
    }
}
