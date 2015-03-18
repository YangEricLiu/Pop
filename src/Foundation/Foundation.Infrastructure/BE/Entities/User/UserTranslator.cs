using System;
using SE.DSP.Foundation.Infrastructure.BaseClass;

using SE.DSP.Foundation.Infrastructure;


using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Enumerations;


namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public static class UserTranslator
    {
        public static UserDto UserEntity2UserDto(UserEntity entity)
        {
            if (entity == null) return null;

            var dto = new UserDto
                {
                    Title = entity.Title,
                    Id = entity.Id,
                    UserType = entity.UserType.HasValue? entity.UserType.Value: 0,
                    Name = entity.Name,
                    Version = entity.Version,
                    Email = entity.Email,
                    Password = entity.Password,
                    RealName = entity.RealName,
                    Telephone = entity.Telephone,
                    Comment = entity.Comment,
                    DemoStatus = entity.DemoStatus,
                    SpId = entity.SpId,
                    CustomerIds = new long[0]
                };

            //dto.RoleIds = entity.RoleIds;
            //dto.CustomerIds = entity.CustomerIds;
            //dto.UserType = entity.UserType;

            if (entity.RoleIds != null && entity.RoleIds.Length > 0) dto.UserType = entity.RoleIds[0];

            return dto;
        }

        public static UserEntity UserDto2UserEntity(UserDto dto)
        {
            if (dto == null) return null;

            var entity = new UserEntity
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Version = dto.Version,
                    UserType = dto.UserType,
                    Title = dto.Title,
                    Email = dto.Email,
                    Password = dto.Password,
                    RealName = dto.RealName,
                    Telephone = dto.Telephone,
                    Comment = dto.Comment,
                    DemoStatus = dto.DemoStatus,
                    SpId = dto.SpId,
                    Status = EntityStatus.Active,
                    UpdateUser = ServiceContext.CurrentUser.Name,
                    UpdateTime = DateTime.UtcNow,
                };


            if (dto.UserType != -1)
            {
                entity.RoleIds = new[] { dto.UserType };
            }

            //entity.PasswordTokenDate = System.DateTime.UtcNow;
            //entity.CustomerIds = dto.CustomerIds;
            //entity.UserType = dto.UserType;

            return entity;
        }

        public static UserFilter UserFilterDto2UserFilter(UserFilterDto dto)
        {
            if (dto == null) return null;

            return new UserFilter
                {
                    Name = dto.Name,
                    UserIds = dto.UserIds,
                    CustomerIds = dto.CustomerId.HasValue ? new[] { dto.CustomerId.Value } : new long[0],
                    RoleIds = dto.UserType.HasValue ? new [] {dto.UserType.Value} : new long[0],
                    DemoStatus = dto.DemoStatus,
                    ExcludeId = dto.ExcludeId,
                    SpId = dto.SpId,
                };
        }

        public static RoleFilter UserRoleFilterDto2UserRoleFilter(UserRoleFilterDto dto)
        {
            return dto == null ? null : new RoleFilter {UserIds = dto.UserIds, RoleIds = dto.RoleIds};
        }
    }
}