

using SE.DSP.Foundation.Infrastructure.Utils;
using System.Collections.Generic;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class ZoneTranslator
    {
        public static ZoneDto Entity2Dto(ZoneEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            else
            {
                ZoneDto dto = Translator.Entity2Dto<ZoneEntity, ZoneDto>(entity);

                dto.Code = entity.Code;
                dto.ParentId = entity.ParentId;
                dto.Comment = entity.Comment;

                return dto;
            }
        }

        public static ZoneDto[] Entities2Dtos(ZoneEntity[] entities)
        {
            if (entities == null)
            {
                return null;
            }
            else
            {
                List<ZoneDto> dtoList = new List<ZoneDto>();

                foreach (ZoneEntity entity in entities)
                {
                    dtoList.Add(Entity2Dto(entity));
                }

                return dtoList.ToArray();
            }
        }

        public static ZoneEntity Dto2Entity(ZoneDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            else
            {
                ZoneEntity entity = Translator.Dto2Entity<ZoneDto, ZoneEntity>(dto);

                entity.Code = dto.Code;
                entity.ParentId = dto.ParentId;
                entity.Comment = dto.Comment;

                return entity;
            }
        }

        public static ZoneEntity[] Dtos2Entities(ZoneDto[] dtoes)
        {
            if (dtoes == null)
            {
                return null;
            }
            else
            {
                List<ZoneEntity> entityList = new List<ZoneEntity>();

                foreach (ZoneDto dto in dtoes)
                {
                    entityList.Add(Dto2Entity(dto));
                }

                return entityList.ToArray();
            }
        }
    }
}
