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
    public class ScenePictureRepository : Repository<ScenePicture, long>, IScenePictureRepository
    {
        public override ScenePicture GetById(long id)
        {
            var result = this.Db.SingleOrDefault<ScenePicture>("where Id=@0", id);

            return result;
        }

        public override ScenePicture Add(ScenePicture entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(ScenePicture entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override ScenePicture Add(IUnitOfWork unitOfWork, ScenePicture entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("ScenePicture", "Id", entity);

            entity.Id = (long)id;

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, ScenePicture entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("ScenePicture", "Id", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<Logo>(id);
        }

        public ScenePicture[] GetScenePictureByHierarchyId(long hierarchyId)
        {
            var result = this.Db.Query<ScenePicture>("where hierarchyId = @0", hierarchyId);

            return result.ToArray();
        }
    }
}
