using SE.DSP.Foundation.API;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.ActionExtension;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using SE.DSP.Foundation.Infrastructure.Constant;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Practices.Unity;

namespace SE.DSP.Foundation.AppHost.API
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceProvider : ServiceBase, IServiceProviderService
    {
        #region DI

        private IServiceProviderDA _spApi;

        private IUserService _userBL;
        public IUserService UserBL
        {
            get { return _userBL ?? (_userBL = IocHelper.Container.Resolve<IUserService>()); }
            set { _userBL = value; }
        }
        public IServiceProviderDA ServiceProviderAPI
        {
            get { return _spApi ?? (_spApi = IocHelper.Container.Resolve<IServiceProviderDA>()); }
        }

        #endregion

        public ServiceProviderDto CreateServiceProvider(ServiceProviderDto dto)
        {
            var entity = ServiceProviderDto.ToEntity(dto);

            if (!UserBL.ValidateName(new UserDto { Name = dto.UserName }))
            {
                throw new BusinessLogicException(Layer.BL, Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.UserNameIsDuplicated));
            }

            using (var ts = TransactionHelper.CreateSerializable())
            {
                entity.Id = ServiceProviderAPI.CreateServiceProvider(entity);
                ServiceProviderAPI.UpdateServiceProvider(entity);

                var sps = ServiceProviderAPI.RetrieveServiceProviders(new ServiceProviderFilter { Ids = new[] { entity.Id.Value } });
                var sp = sps[0];

                ts.Complete();
                return ServiceProviderDto.FromEntity(sp);
            }
        }
        public ServiceProviderDto ModifyServiceProvider(ServiceProviderDto dto)
        {
            var entity = ServiceProviderDto.ToEntity(dto);

            bool lastCalcStatus = false;

            using (var ts = TransactionHelper.CreateSerializable())
            {
                var sps = ServiceProviderAPI.RetrieveServiceProviders(new ServiceProviderFilter { Ids = new[] { dto.Id.Value } });
                if (sps.Length == 0)
                {
                    throw new ConcurrentException(Layer.BL,
                        Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderHasBeenDeleted));
                }
                var sp = sps[0];
                lastCalcStatus = sp.CalcStatus;

                if (sp.Version != dto.Version)
                {
                    throw new ConcurrentException(Layer.BL,
                        Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderHasBeenModified));
                }
                entity.UserName = sp.UserName;
                //  entity.CalcStatus = dto.CalcStatus;

                if (sp.DeployStatus == DeployStatus.Finished && entity.DeployStatus == DeployStatus.Processing)
                {
                    entity.DeployStatus = sp.DeployStatus;
                }

                var users = UserBL.RetrieveUserEntitiesByFilter(new UserFilter { SpId = -1, Name = dto.UserName });
                if (users != null && users.Length > 0 && users[0] != null)
                {
                    var user = users[0];
                    if (user.Email != dto.Email)
                    {
                        user.Email = dto.Email;
                        user.Telephone = dto.Telephone;
                        UserBL.UpdateUser(user);
                    }
                }

                ServiceProviderAPI.UpdateServiceProvider(entity);
                sps = ServiceProviderAPI.RetrieveServiceProviders(new ServiceProviderFilter { Ids = new[] { dto.Id.Value } });
                sp = sps[0];

                ts.Complete();
                ts.Dispose();

                if (dto.CalcStatus != lastCalcStatus)
                {
                    var dict = new Dictionary<string, string>();
                    dict.Add("Id", sp.Id.ToString());
                    ActionExcusor.Fire("ServiceProviderService.ModifyServiceProvider", false, PositionType.After, dict);
                }

                return ServiceProviderDto.FromEntity(sp);
            }
        }
        public void UpdateServiceProvider(ServiceProviderEntity entity)
        {
            ServiceProviderAPI.UpdateServiceProvider(entity);
        }
        public ServiceProviderDto SendInitPassword(long spId)
        {
            //using (var ts = TransactionHelper.CreateRepeatableRead())
            //{

            UserBL.SendSPInitPassword(spId);

            //Dictionary<string,string> dict=new Dictionary<string,string>();
            //dict.Add("spId",spId.ToString());
            //ActionExcusor.Fire("ServiceProvider.SendInitPassword", 
            //    false, 
            //    PositionType.Before,
            //    dict);

            var sps = ServiceProviderAPI.RetrieveServiceProviders(new ServiceProviderFilter { Ids = new[] { spId } });
            if (sps.Length > 0)
            {
                var sp = sps[0];
                return ServiceProviderDto.FromEntity(sp);
            }

            return null;
            //ts.Complete();

            //UserBL.SendSPInitPassword(spId);
            //ts.Complete();
            //return ServiceProviderDto.FromEntity(sp);
            //}
        }
        public void DeleteServiceProvider(DtoBase dto)
        {
            var sps = ServiceProviderAPI.RetrieveServiceProviders(new ServiceProviderFilter { Ids = new[] { dto.Id.Value } });
            if (sps.Length == 0) return;
            var sp = sps[0];

            if (sp.Version != dto.Version)
            {
                throw new ConcurrentException(Layer.BL,
                    Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderHasBeenModified));
            }

            if (sp.DeployStatus == DeployStatus.Finished)
            {
                // TODO: CONN
                var users = UserBL.RetrieveUserEntitiesByFilter(new UserFilter { SpId = dto.Id.Value });
                foreach (var item in users)
                {
                    UserBL.DeleteUserEntity(item.Id.Value);
                }

                //Dictionary<string, string> dict = new Dictionary<string, string>();

                //dict.Add("DbType", REMDbType.ServiceProvider.ToString());
                //dict.Add("ConnectionString", conn);

                ////todo:neeed implement interface 
                //ActionExcusor.Fire("ServiceProviderService.DeleteServiceProvider", false, PositionType.Before, dict);

                sp.Status = EntityStatus.Deleted;
                ServiceProviderAPI.UpdateServiceProvider(sp);
            }
            else
            {
                sp.Status = EntityStatus.Deleted;
                ServiceProviderAPI.UpdateServiceProvider(sp);
            }
        }

        public ServiceProviderDto[] GetServiceProviders(ServiceProviderFilter filter)
        {
            if (filter == null) return null;
            if (filter.StatusFilter == null)
            {
                filter.StatusFilter = GetUndeletedStatusFilter();
            }

            var entities = ServiceProviderAPI.RetrieveServiceProviders(filter);
            return entities.Select(ServiceProviderDto.FromEntity).ToArray();
        }

        public long GetMaxServiceProviderId()
        {
            var filter = new ServiceProviderFilter { StatusFilter = new StatusFilter { } };
            var entities = ServiceProviderAPI.RetrieveServiceProviders(filter);
            if (entities.Length > 0) return 0;
            var maxId = entities.Select(p => p.Id.Value).Max();
            return maxId;
        }

        public ServiceProviderCalcInfo[] GetServiceProviderCalcInfos()
        {
            var filter = new ServiceProviderFilter { StatusFilter = new StatusFilter { } };
            var entities = ServiceProviderAPI.RetrieveServiceProviders(filter);

            var list = new List<ServiceProviderCalcInfo>();

            foreach (var item in entities)
            {
                if (item.DeployStatus == DeployStatus.Processing) continue;

                list.Add(new ServiceProviderCalcInfo { Id = item.Id.Value });
            }
            return list.ToArray();
        }

        /// <summary>
        /// Get available status 
        /// </summary>
        /// <returns></returns>
        protected static StatusFilter GetUndeletedStatusFilter()
        {
            return new StatusFilter { ExcludeStatus = true, Statuses = new[] { EntityStatus.Deleted } };
        }

        public ParentCalcStatus GetSpCalcStatus(long spId)
        {

            var spArray = GetServiceProviders(new ServiceProviderFilter { Ids = new long[] { spId } });

            if (spArray == null || spArray.Length == 0)
                return ParentCalcStatus.SPNotCalculated;

            var spInfo = spArray.First();

            if (spInfo.CalcStatus)
                return ParentCalcStatus.SPCalculated;
            else
                return ParentCalcStatus.SPNotCalculated;

        }
    }
}
