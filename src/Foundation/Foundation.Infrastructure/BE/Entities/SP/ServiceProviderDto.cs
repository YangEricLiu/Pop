using SE.DSP.Foundation.Infrastructure.BaseClass;


using System;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using System.Runtime.Serialization;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    [DataContract]
    public class ServiceProviderDto : DtoBase
    {
        [DataMember]
        public String UserName { get; set; }
        [DataMember]
        public String Address { get; set; }
        [DataMember]
        public String Telephone { get; set; }
        [DataMember]
        public String Email { get; set; }
        [DataMember]
        public String Domain { get; set; }
        [DataMember]
        public String Comment { get; set; }

        private DateTime? _startDate;
        [DataMember]
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { _startDate = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : value; }
        }

        [DataMember]
        public EntityStatus Status { get; set; }
        [DataMember]
        public DeployStatus DeployStatus { get; set; }
        [DataMember]
        public String UpdateUser { get; set; }
        [DataMember]
        public DateTime UpdateTime { get; set; }


        [DataMember]
        public bool CalcStatus { set; get; }

        public static ServiceProviderEntity ToEntity(ServiceProviderDto dto)
        {
            if (dto == null) return null;
            return new ServiceProviderEntity
            {
                Address = dto.Address,

                Email = dto.Email,
                StartDate = dto.StartDate,
                Telephone = dto.Telephone,
                UserName = dto.UserName,
                Id = dto.Id,
                Name = dto.Name,
                Version = dto.Version,
                Comment = dto.Comment,
                Status = dto.Status,
                Domain = dto.Domain,
                DeployStatus = dto.DeployStatus,
                CalcStatus = dto.CalcStatus,

                UpdateTime = dto.UpdateTime == DateTime.MinValue ? DateTime.UtcNow : dto.UpdateTime,
                UpdateUser = dto.UpdateUser ?? ServiceContext.CurrentUser.Name,
            };
        }

        public static ServiceProviderDto FromEntity(ServiceProviderEntity entity)
        {
            if (entity == null) return null;
            return new ServiceProviderDto
            {
                Address = entity.Address,

                Email = entity.Email,
                StartDate = entity.StartDate,
                Telephone = entity.Telephone,
                UserName = entity.UserName,
                Id = entity.Id,
                Domain = entity.Domain,
                Name = entity.Name,
                Version = entity.Version,
                Comment = entity.Comment,
                Status = entity.Status,
                UpdateTime = entity.UpdateTime,
                UpdateUser = entity.UpdateUser,
                DeployStatus = entity.DeployStatus,
                CalcStatus = entity.CalcStatus
            };
        }
    }

    public class ServiceProviderCalcInfo
    {
        public long Id { get; set; }
        public String AppPath { get; set; }
    }
}
