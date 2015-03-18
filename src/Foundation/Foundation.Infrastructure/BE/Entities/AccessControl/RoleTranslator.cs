using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure;

using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Interception;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public static class RoleTranslator
    {
        public static RoleDto RoleEntity2RoleDto(RoleEntity entity)
        {
            if (entity == null) return null;

            var dto = Translator.Entity2Dto<RoleEntity, RoleDto>(entity);
            dto.SpId = entity.SpId;
            
            return dto;
        }

        public static RoleEntity RoleDto2RoleEntity(RoleDto dto)
        {
            if (dto == null) return null;

            var entity = Translator.Dto2Entity<RoleDto, RoleEntity>(dto);
            entity.SpId = ServiceContext.CurrentUser.SPId;
            
            return entity;
        }
    }
}