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
    public class SceneLogRepository : Repository<SceneLog, long>, ISceneLogRepository
    {
        public override SceneLog GetById(long id)
        {
            var result = this.Db.SingleOrDefault<SceneLog>("where Id=@0", id);

            return result;
        }

        public override SceneLog Add(SceneLog entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(SceneLog entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override SceneLog Add(IUnitOfWork unitOfWork, SceneLog entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("SceneLog", "Id", entity);

            entity.Id = (long)id;

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, SceneLog entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("SceneLog", "Id", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<SceneLog>(id);
        }

        public SceneLog[] GetSceneLogByHierarchyId(long hierarchyId)
        {
            var result = this.Db.Query<SceneLog>("where hierarchyId = @0", hierarchyId);

            return result.ToArray();
        }
    }
}
