using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.BE.Entities;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class CommodityTranslator
    {
        public static CommodityDto[] CommodityEntities2CommodityDtoes(CommodityEntity[] entities)
        {
            if (entities == null)
            {
                return null;
            }
            else
            {
                List<CommodityDto> dtoList = new List<CommodityDto>();

                foreach (CommodityEntity entity in entities)
                {
                    dtoList.Add(CommodityEntity2CommodityDto(entity));
                }

                return dtoList.ToArray();
            }
        }
        
        public static CommodityDto CommodityEntity2CommodityDto(CommodityEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            else
            {
                CommodityDto dto = Translator.Entity2Dto<CommodityEntity, CommodityDto>(entity);

                dto.Code = entity.Code;
                dto.Comment = entity.Comment;

                return dto;
            }
        }

        public static CommodityDto CommodityEntity2CommodityDto(CommodityEntity entity,UomGroupRelationEntity[] uoms)
        {
            if (entity == null)
            {
                return null;
            }
            else
            {
                CommodityDto dto = Translator.Entity2Dto<CommodityEntity, CommodityDto>(entity);

                dto.Code = entity.Code;
                dto.Comment = entity.Comment;
                dto.Uoms = (from u in uoms select new UomDto() { Id = u.UomId, Code = u.Uom.Code, Comment = u.Uom.Comment, Precision = u.Precision, Version = u.Uom.Version }).ToArray();

                return dto;
            }
        }

        public static CommodityEntity[] CommodityDtoes2CommodityEntities(CommodityDto[] dtoes)
        {
            if (dtoes == null)
            {
                return null;
            }
            else
            {
                List<CommodityEntity> entityList = new List<CommodityEntity>();

                foreach (CommodityDto dto in dtoes)
                {
                    entityList.Add(CommodityDto2CommodityEntity(dto));
                }

                return entityList.ToArray();
            }
        }

        public static CommodityEntity CommodityDto2CommodityEntity(CommodityDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            else
            {
                CommodityEntity entity = Translator.Dto2Entity<CommodityDto, CommodityEntity>(dto);

                entity.Code = dto.Code;
                entity.Comment = dto.Comment;

                return entity;
            }
        }
    }
}
