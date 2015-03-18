
using SE.DSP.Foundation.Infrastructure.Utils;
using System.Collections.Generic;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public static class IndustryTranslator
    {
        public static IndustryDto Entity2Dto(IndustryEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            else
            {
                var dto = Translator.Entity2Dto<IndustryEntity, IndustryDto>(entity);

                dto.Code = entity.Code;
                dto.ParentId = entity.ParentId;
                dto.Comment = entity.Comment;

                return dto;
            }
        }

        public static IndustryDto[] Entities2Dtos(IndustryEntity[] entities)
        {
            if (entities == null)
            {
                return null;
            }
            else
            {
                var dtoList = new List<IndustryDto>();

                foreach (var entity in entities)
                {
                    dtoList.Add(Entity2Dto(entity));
                }

                return dtoList.ToArray();
            }
        }

        public static IndustryEntity Dto2Entity(IndustryDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            else
            {
                IndustryEntity entity = Translator.Dto2Entity<IndustryDto, IndustryEntity>(dto);

                entity.Code = dto.Code;
                entity.ParentId = dto.ParentId;
                entity.Comment = dto.Comment;

                return entity;
            }
        }

        public static IndustryEntity[] Dtos2Entities(IndustryDto[] dtoes)
        {
            if (dtoes == null)
            {
                return null;
            }
            else
            {
                List<IndustryEntity> entityList = new List<IndustryEntity>();

                foreach (IndustryDto dto in dtoes)
                {
                    entityList.Add(Dto2Entity(dto));
                }

                return entityList.ToArray();
            }
        }
    }
}
