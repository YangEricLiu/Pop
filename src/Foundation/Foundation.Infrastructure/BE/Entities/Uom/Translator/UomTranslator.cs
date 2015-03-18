using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.BE.Entities;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UomTranslator
    {
        public static UomDto[] UomEntities2UomDtoes(UomEntity[] entities)
        {
            if (entities == null)
            {
                return null;
            }
            else
            {
                List<UomDto> dtoList = new List<UomDto>();

                foreach (UomEntity entity in entities)
                {
                    dtoList.Add(UomEntity2UomDto(entity));
                }

                return dtoList.ToArray();
            }
        }

        public static UomDto UomEntity2UomDto(UomEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            else
            {
                UomDto dto = Translator.Entity2Dto<UomEntity, UomDto>(entity);

                dto.Code = entity.Code;
                dto.Comment = entity.Comment;

                return dto;
            }
        }

        public static UomEntity[] UomDtoes2UomEntities(UomDto[] dtoes)
        {
            if (dtoes == null)
            {
                return null;
            }
            else
            {
                List<UomEntity> entityList = new List<UomEntity>();

                foreach (UomDto dto in dtoes)
                {
                    entityList.Add(UomDto2UomEntity(dto));
                }

                return entityList.ToArray();
            }
        }

        public static UomEntity UomDto2UomEntity(UomDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            else
            {
                UomEntity entity = Translator.Dto2Entity<UomDto, UomEntity>(dto);

                entity.Code = dto.Code;
                entity.Comment = dto.Comment;

                return entity;
            }
        }
    }
}
