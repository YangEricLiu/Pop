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
    public class SingleLineDiagramRepository : Repository<SingleLineDiagram, long>, ISingleLineDiagramRepository
    {
        public override SingleLineDiagram GetById(long id)
        {
            var result = this.Db.SingleOrDefault<SingleLineDiagram>("where Id=@0", id);

            return result;
        }

        public SingleLineDiagram[] GetByHierarchyId(long hierarchyId)
        {
            var result = this.Db.Query<SingleLineDiagram>("where hierarchyId = @0", hierarchyId);

            return result.ToArray();
        }

        public override SingleLineDiagram Add(SingleLineDiagram entity)
        {
            return this.Add(null, entity);
        }

        public override void Update(SingleLineDiagram entity)
        {
            this.Update(null, entity);
        }

        public override void Delete(long id)
        {
            this.Delete(null, id);
        }

        public override SingleLineDiagram Add(IUnitOfWork unitOfWork, SingleLineDiagram entity)
        {
            var db = this.GetDatabese(unitOfWork);

            var id = db.Insert("SingleLineDiagram", "Id", entity);

            entity.Id = (long)id;

            return entity;
        }

        public override void Update(IUnitOfWork unitOfWork, SingleLineDiagram entity)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Save("SingleLineDiagram", "Id", entity);
        }

        public override void Delete(IUnitOfWork unitOfWork, long id)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<SingleLineDiagram>(id);
        }

        public void DeleteByHierarchyId(IUnitOfWork unitOfWork, long hierarchyId)
        {
            var db = this.GetDatabese(unitOfWork);

            db.Delete<SingleLineDiagram>("where hierarchyId = @0", hierarchyId);
        }
    }
}
