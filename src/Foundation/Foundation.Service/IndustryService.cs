using Microsoft.Practices.Unity;
using SE.DSP.Foundation.API;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Utils;
using System.Linq;

namespace SE.DSP.Foundation.Service
{
    public class IndustryService : SE.DSP.Foundation.Infrastructure.BaseClass.ServiceBase, IIndustryService
    {
        #region DI
        private IIndustryDA _industryAPI;

        public IIndustryDA IndustryAPI
        {
            get
            {
                return this._industryAPI ?? (this._industryAPI = IocHelper.Container.Resolve<IIndustryDA>());
            }

            set
            {
                this._industryAPI = value;
            }
        }
 
        #endregion

        /// <summary>
        /// Get the specified industry by its id
        /// </summary>
        /// <param name="industryId">id of the desired industry</param>
        /// <returns></returns>
        public IndustryDto GetIndustryById(long industryId)
        {
            var entity = this.IndustryAPI.RetrieveIndustryById(industryId);

            return IndustryTranslator.Entity2Dto(entity);
        }

        /// <summary>
        /// Get all industries defined in the system
        /// </summary>
        /// <returns></returns>
        public IndustryDto[] GetAllIndustries(bool onlyLeaf, bool includeRoot)
        {
            var entities = this.IndustryAPI.RetrieveAllIndustries();

            if (onlyLeaf)
            {
                var parentIds = entities.Where(pEntity => pEntity.ParentId.HasValue).Select(pEntity => pEntity.ParentId.Value);

                var leafIndustries = entities.Where(entity => !parentIds.Contains(entity.Id.Value));

                entities = leafIndustries.ToArray();
            }

            if (!includeRoot)
            {
                entities = entities.Where(entity => entity.ParentId.HasValue).ToArray();
            }

            return IndustryTranslator.Entities2Dtos(entities);
        }
    }
}