/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: Translator.cs
 * Author	    : Figo
 * Date Created : 2012-03-01
 * Description  : Translates between EntityBase and DtoBase />
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using System;


namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// Translate between <see cref="EntityBase" /> and <see cref="DtoBase" />
    /// </summary>
    public static class Translator
    {
        /// <summary>
        /// Translates from <see cref="EntityBase" /> to <see cref="DtoBase" />
        /// </summary>
        /// <param name="entity">The EntityBase object.</param>
        /// <returns>The DtoBase Object.</returns>
        public static T2 Entity2Dto<T1, T2>(T1 entity)
            where T1 : EntityBase, new()
            where T2 : DtoBase, new()
        {
            return entity == null ? null : new T2()
            {
                Id = entity.Id,
                Name = entity.Name,
                Version = entity.Version
            };
        }

        /// <summary>
        /// Translates from <see cref="DtoBase" /> to <see cref="EntityBase" />
        /// </summary>
        /// <param name="dto">The DtoBase Object.</param>
        /// <returns>The EntityBase object.</returns>
        public static T2 Dto2Entity<T1, T2>(T1 dto)
            where T1 : DtoBase, new()
            where T2 : EntityBase, new()
        {
            return dto == null ? null : new T2()
            {
                Id = dto.Id,
                Name = dto.Name,
                Status = EntityStatus.Active,
                UpdateUser = ServiceContext.CurrentUser.Name,
                UpdateTime = DateTime.UtcNow,
                Version = dto.Version,
                
            };
        }
    }
}