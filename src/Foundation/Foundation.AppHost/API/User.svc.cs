using SE.DSP.Foundation.API;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.ActionExtension;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using SE.DSP.Foundation.Infrastructure.Constant;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SpreadsheetGear;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Practices.Unity;

namespace SE.DSP.Foundation.AppHost.API
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class User : ServiceBase, IUserService
    {
        #region DI

        private IUserDA _userApi;
        public IUserDA UserAPI
        {
            get { return _userApi ?? (_userApi = IocHelper.Container.Resolve<IUserDA>()); }
            set { _userApi = value; }
        }

        private IRoleDA _roleApi;
        public IRoleDA RoleAPI
        {
            get { return _roleApi ?? (_roleApi = IocHelper.Container.Resolve<IRoleDA>()); }
            set { _roleApi = value; }
        }

        private IRolePrivilegeDA _rolepApi;
        public IRolePrivilegeDA RolePrivilegeAPI
        {
            get { return _rolepApi ?? (_rolepApi = IocHelper.Container.Resolve<IRolePrivilegeDA>()); }
            set { _rolepApi = value; }
        }

        private IAccessControlService _accessControlBL;
        public IAccessControlService AccessControlBL
        {
            get { return _accessControlBL ?? (_accessControlBL = IocHelper.Container.Resolve<IAccessControlService>()); }
            set { _accessControlBL = value; }
        }

        private IServiceProviderService _ServiceProviderBL;
        public IServiceProviderService ServiceProviderBL
        {
            get { return _ServiceProviderBL ?? (_ServiceProviderBL = IocHelper.Container.Resolve<IServiceProviderService>()); }
        }

        #endregion

        public string ProductInfo
        {
            get
            {
                return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "BLCOMMON_ProductInfo");
            }
        }

        static DateTime InitPasswordExpirationTime = new DateTime(Constant.INITPASSWORDTOKENEXPIRATIONABSOLUTEYEAR, 1, 1);
        public UserDto CreateUser(UserDto dto)
        {
            UserEntity entity;
            dto.SpId = ServiceContext.CurrentUser.SPId;

            //Validate Customer code & name
            ValidateRealName(dto);
            if (!ValidateName(dto)) ThrowBusinessLogicException(UserErrorCode.UserNameIsDuplicated);

            // Put the logical into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateSerializable())
            {
                entity = UserTranslator.UserDto2UserEntity(dto);
                entity.Password = CryptographyHelper.MD5(Util.GenerateRandom());
                var entityId = UserAPI.CreateUser(entity);

                var userRoles = new UserRoleEntity[entity.RoleIds.Length];
                for (int i = 0; i < entity.RoleIds.Length; i++)
                {
                    userRoles[i] = new UserRoleEntity { UserId = entityId, RoleId = entity.RoleIds[i] };
                }

                UserAPI.CreateUserRole(userRoles);

                var newEntity = UserAPI.RetrieveUserById(entityId);

                if (entity.UserType != -1)
                {
                    newEntity.RoleIds = entity.RoleIds;
                }
                entity = newEntity;
                ts.Complete();
            }
            return UserTranslator.UserEntity2UserDto(entity);
        }
        public UserDto ModifyUser(UserDto dto)
        {
            var entity = UserTranslator.UserDto2UserEntity(dto);
            UserCustomerEntity[] customers;
            var roles = new List<UserRoleEntity>();

            //Validate Customer code & name
            ValidateRealName(dto);
            //if (!ValidateName(dto)) ThrowBusinessLogicException(UserErrorCode.UserNameIsDuplicated);

            // Put the logical into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateSerializable())
            {
                var oringalUser = UserAPI.RetrieveUserById(dto.Id.Value);
                if (oringalUser == null) ThrowConcurrentException(UserErrorCode.UserDoesNotExist);
                if (oringalUser.Version != dto.Version) ThrowConcurrentException(UserErrorCode.UserIsExpired);

                var sps = ServiceProviderBL.GetServiceProviders(new ServiceProviderFilter { Ids = new[] { oringalUser.SpId } });
                if (sps != null && sps.Length > 0 && sps[0] != null)
                {
                    var sp = sps[0];
                    if (sp.UserName == oringalUser.Name)
                    {
                        var spentity = ServiceProviderDto.ToEntity(sp);
                        spentity.Email = dto.Email;
                        spentity.Telephone = dto.Telephone;
                        ServiceProviderBL.UpdateServiceProvider(spentity);
                    }
                }

                entity.Name = oringalUser.Name;
                entity.DemoStatus = oringalUser.DemoStatus;
                entity.SpId = oringalUser.SpId;
                entity.UserType = oringalUser.UserType;

                if (entity.DemoStatus != 0)
                {
                    entity.Name = oringalUser.Name;
                    entity.RealName = oringalUser.RealName;
                }

                entity.Password = oringalUser.Password;
                entity.PasswordToken = oringalUser.PasswordToken;
                entity.PasswordTokenDate = oringalUser.PasswordTokenDate;

                UserAPI.UpdateUser(entity);

                if (entity.UserType != -1)
                {
                    roles.AddRange(entity.RoleIds.Select(role => new UserRoleEntity { UserId = dto.Id.Value, RoleId = role }));

                    UserAPI.DeleteUserRole(new RoleFilter { UserIds = new[] { dto.Id.Value } });
                    UserAPI.CreateUserRole(roles.ToArray());
                }

                var newEntity = UserAPI.RetrieveUserById(dto.Id.Value);

                if (entity.UserType != -1)
                {
                    newEntity.RoleIds = entity.RoleIds;
                }

                customers = UserAPI.RetrieveUserCustomers(new UserCustomerFilter { UserIds = new[] { dto.Id.Value } });
                entity = newEntity;
                ts.Complete();
            }
            var rtnval = UserTranslator.UserEntity2UserDto(entity);
            rtnval.CustomerIds = customers.Select(p => p.CustomerId).ToArray();
            return rtnval;
        }
        public UserDto ModifyProfile(UserDto dto)
        {
            if (dto.Id.Value != ServiceContext.CurrentUser.Id) ThrowBusinessLogicException(UserErrorCode.UserCannotModifyOthersProfile);
            var entity = UserTranslator.UserDto2UserEntity(dto);

            //Validate Customer code & name
            ValidateRealName(dto);
            //if (!ValidateName(dto)) ThrowBusinessLogicException(UserErrorCode.UserNameIsDuplicated);

            // Put the logical into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateSerializable())
            {
                var oringalUser = UserAPI.RetrieveUserById(dto.Id.Value);
                if (oringalUser == null) ThrowConcurrentException(UserErrorCode.UserDoesNotExist);
                if (oringalUser.Version != dto.Version) ThrowConcurrentException(UserErrorCode.UserIsExpired);

                var sps = ServiceProviderBL.GetServiceProviders(new ServiceProviderFilter { Ids = new[] { oringalUser.SpId } });
                if (sps != null && sps.Length > 0 && sps[0] != null)
                {
                    var sp = sps[0];
                    if (sp.UserName == oringalUser.Name && sp.Email != oringalUser.Email)
                    {
                        var spentity = ServiceProviderDto.ToEntity(sp);
                        sp.Email = dto.Email;
                        sp.Telephone = dto.Telephone;
                        ServiceProviderBL.UpdateServiceProvider(spentity);
                    }
                }

                entity.DemoStatus = oringalUser.DemoStatus;
                if (entity.DemoStatus != 0)
                {
                    entity.Name = oringalUser.Name;
                }
                entity.SpId = oringalUser.SpId;
                entity.Name = oringalUser.Name;
                entity.Password = oringalUser.Password;
                entity.PasswordToken = oringalUser.PasswordToken;
                entity.PasswordTokenDate = oringalUser.PasswordTokenDate;
                entity.UserType = oringalUser.UserType;
                UserAPI.UpdateUser(entity);

                var newEntity = UserAPI.RetrieveUserById(dto.Id.Value);

                if (entity.UserType != -1)
                {
                    newEntity.RoleIds = entity.RoleIds;
                }
                entity = newEntity;

                ts.Complete();
            }
            var rtndto = UserTranslator.UserEntity2UserDto(entity);

            //long spid = 0;
            //var spIdStr = ConfigHelper.Get(BLConfiguratgionKey.SPID);
            //long.TryParse(spIdStr, out spid);
            //rtndto.SpId = spid;
            return rtndto;
        }
        public void DeleteUser(DtoBase dto)
        {
            if (dto == null || !dto.Id.HasValue) return;

            if (dto.Id == Constant.INITPLATFORMADMINISTRATOR)
            {
                ThrowBusinessLogicException(UserErrorCode.DefualtPlatformAdministratorCantBeDeleted);
            }
            if (dto.Id == ServiceContext.CurrentUser.Id)
            {
                ThrowBusinessLogicException(UserErrorCode.UserCannotDeleteOneself);
            }

            // Put the logical into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateSerializable())
            {
                var oringalUser = UserAPI.RetrieveUserById(dto.Id.Value);
                if (oringalUser == null) return;
                if (oringalUser.Version != dto.Version) ThrowConcurrentException(UserErrorCode.UserIsExpired);

                var sps = ServiceProviderBL.GetServiceProviders(new ServiceProviderFilter { Ids = new[] { oringalUser.SpId } });
                if (sps != null && sps.Length > 0 && sps[0] != null)
                {
                    var sp = sps[0];
                    if (sp.UserName == oringalUser.Name)
                        ThrowBusinessLogicException(UserErrorCode.DefualtPlatformAdministratorCantBeDeleted);
                }

                var dict = new Dictionary<string, string>();
                dict.Add("userId", dto.Id.Value.ToString());
                ActionExcusor.Fire("UserService.DeleteUser", true, PositionType.Before, dict);

                //todo: will add extension code here
                // Remove dashboard
                //DashBoardBL.DeleteAllDashboardByUserId(dto.Id.Value);


                //// Remove dashboard
                //CollaborativeWidgetBL.DeleteWidgetInfosByUserId(dto.Id.Value);

                ////remove alarm subcriber
                //this.AlarmBL.DeleteAlarmSubscriber(dto.Id.Value);

                // Remove data privilege
                AccessControlBL.DeleteDataPrivileges(new DataPrivilegeFilter { UserIds = new[] { dto.Id.Value } });

                // Delete UserCustomer
                UserAPI.DeleteUserCustomer(new UserCustomerFilter { UserIds = new[] { dto.Id.Value } });
                UserAPI.DeleteUserRole(new RoleFilter { UserIds = new[] { dto.Id.Value }, SpId = ServiceContext.CurrentUser.SPId });

                // Delete User
                UserAPI.DeleteUser(new UserFilter { UserIds = new[] { dto.Id.Value }, SpId = ServiceContext.CurrentUser.SPId });

                ts.Complete();
            }
        }
        public DtoBase SendInitPassword(long userId)
        {
            var user = UserAPI.RetrieveUserById(userId);
            if (user == null) ThrowConcurrentException(UserErrorCode.UserDoesNotExist);

            var sps = ServiceProviderBL.GetServiceProviders(new ServiceProviderFilter { Ids = new[] { user.SpId } });
            if (sps.Length == 0)
            {
                throw new ConcurrentException(Layer.BL,
                    Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderHasBeenDeleted));
            }
            var sp = sps[0];
            if (sp.Status == EntityStatus.Inactive)
            {
                throw new BusinessLogicException(Layer.BL,
                    Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderIsPaused));
            }

            user.PasswordToken = Guid.NewGuid().ToString().ToLower();
            user.PasswordTokenDate = InitPasswordExpirationTime;
            UserAPI.UpdateUser(user);

            var sender = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILFROM];
            //var smtpServer = ConfigurationManager.AppSettings["PasswordMailSmtp"];
            var smtpServer = ConfigHelper.Get(DeploymentConfigKey.SmtpServerIP);
            var senderAccount = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILUSERNAME];
            var senderPwd = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILPASSWORD];

            string topic = string.Format(I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_InitPasswordEmailHeader"), this.ProductInfo);
            string webpath = String.Format(ConfigHelper.Get(DeploymentConfigKey.WifAudienceUriTemplate), sp.Domain.ToLower());
            var body = String.Format(
I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_InitPasswordEmailBody"),
user.RealName, user.Name, user.Telephone, user.Email,
String.Format("{0}index.aspx?u={1}&t={2}&a=initpwd&amp;lang=" + I18nHelper.LocaleEnumToString(ServiceContext.Language), webpath, user.Name, user.PasswordToken), UserDto.GetTitle(user.Title), sp.Name, this.ProductInfo);

            MailHelper.Send(
                new[] { user.Email }, sender, topic,
                body, smtpServer,
                senderAccount, senderPwd, new string[0]);

            user = UserAPI.RetrieveUserById(userId);
            return new DtoBase { Id = userId, Version = user.Version };
        }
        public void SendSPInitPassword(long spId)
        {
            ServiceProviderDto sp = null;
            
            ValidateSP(out sp, spId);             

            var previousDeployStatus = sp.DeployStatus;

            try
            {
                //var connstr = GetConnectionString(sp.DbDatabase, sp.DbServer, sp.DbUser, sp.DbPassword);
                //var connOption = new ConnectionOption { ConnectionString = connstr };

                // TODO: CONN
                var users = UserAPI.RetrieveUsers(new UserFilter { Name = sp.UserName, SpId = spId });
                UserEntity user = null;

                if (users.Length == 0)
                {
                    //throw new BusinessLogicException(Layer.BL, Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderDoesNotHaveRelatedAdmin));
                    user = new UserEntity
                    {
                        Name = sp.UserName,
                        Email = sp.Email,
                        RealName = UserTitle.ServiceProviderAdmin.ToString(),
                        Title = UserTitle.ServiceProviderAdmin,
                        UserType = -1,
                        DemoStatus = 0,
                        Telephone = sp.Telephone,
                        Password = CryptographyHelper.MD5(Util.GenerateRandom()),
                        UpdateTime = DateTime.UtcNow,
                        UpdateUser = ServiceContext.CurrentUser.Name,
                        SpId = spId,
                    };
                    user.Id = UserAPI.CreateUser(user);
                    // TODO: CONN                    
                    CreateDefaultData(user.Id.Value, spId);
                }
                else
                {
                    user = users[0];
                }
                var userId = user.Id.Value;

                //if (user == null) ThrowConcurrentException(UserErrorCode.UserDoesNotExist);

                user.PasswordToken = Guid.NewGuid().ToString().ToLower();
                user.PasswordTokenDate = InitPasswordExpirationTime;

                // TODO: CONN
                UserAPI.UpdateUser(user);

                var sender = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILFROM];
                //var smtpServer = ConfigurationManager.AppSettings["PasswordMailSmtp"];
                var smtpServer = ConfigHelper.Get(DeploymentConfigKey.SmtpServerIP);
                var senderAccount = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILUSERNAME];
                var senderPwd = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILPASSWORD];
                string webpath = String.Format(ConfigHelper.Get(DeploymentConfigKey.WifAudienceUriTemplate), sp.Domain.ToLower());
                var body = String.Format(
I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_InitPasswordEmailBody"),
user.RealName, user.Name, user.Telephone, user.Email,
String.Format("{0}index.aspx?u={1}&t={2}&a=initpwd&amp;lang=" + I18nHelper.LocaleEnumToString(ServiceContext.Language), webpath, user.Name, user.PasswordToken), UserDto.GetTitle(user.Title), sp.Name, this.ProductInfo);

                string topic = string.Format(I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_InitPasswordEmailHeader"), this.ProductInfo);

                MailHelper.Send(
                    new[] { user.Email }, sender, topic,
                body, smtpServer,
                    senderAccount, senderPwd, new string[0]);

                sp.DeployStatus = DeployStatus.Finished;
                ServiceProviderBL.ModifyServiceProvider(sp);

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex.Message);
                throw new BusinessLogicException(Layer.BL, Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderIsBeingSetUpForUsage));
            }
        }

        public PasswordDto ResetPassword(PasswordDto dto)
        {
            if (dto == null || !dto.Id.HasValue) return null;
            if (dto.Id.Value != ServiceContext.CurrentUser.Id)
            {
                ThrowBusinessLogicException(UserErrorCode.UserCanOnlyResetPasswordForOneself);
            }

            // Put the logical into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateRepeatableRead())
            {
                var oringalUser = UserAPI.RetrieveUserById(dto.Id.Value);

                if (oringalUser == null) ThrowConcurrentException(UserErrorCode.UserDoesNotExist);
                if (oringalUser.Version != dto.Version) ThrowConcurrentException(UserErrorCode.UserIsExpired);
                //if (oringalUser.PasswordTokenDate == InitPasswordExpirationTime) ThrowBusinessLogicException(UserErrorCode.ActionIsProhibited);

                if (CryptographyHelper.MD5(dto.Password).ToLower() != oringalUser.Password.ToLower())
                {
                    ThrowBusinessLogicException(UserErrorCode.PasswordIsIncorrect);
                }
                oringalUser.Password = CryptographyHelper.MD5(dto.NewPassword);
                oringalUser.PasswordTokenDate = DateTime.UtcNow.AddDays(-1);
                UserAPI.UpdateUser(oringalUser);
                var entity = UserAPI.RetrieveUserById(dto.Id.Value);
                ts.Complete();
                dto.Password = dto.NewPassword;
                dto.Version = entity.Version;
            }
            return dto;
        }
        public void SetInitPassword(PasswordResetDto dto)
        {
            // Put the logic into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateRepeatableRead())
            {
                var users = UserAPI.RetrieveUsers(new UserFilter { Name = dto.UserName, SpId = -1 });
                if (users == null || users.Length == 0) ThrowBusinessLogicException(UserErrorCode.UserNameIsDeleted);

                var user = users[0];
                if (user.PasswordToken != dto.PasswordToken) ThrowBusinessLogicException(UserErrorCode.PasswordTokenDontMatch);
                if (user.PasswordTokenDate < DateTime.UtcNow) ThrowBusinessLogicException(UserErrorCode.PasswordTokenIsExpired);
                if (user.PasswordTokenDate.HasValue && user.PasswordTokenDate != InitPasswordExpirationTime) ThrowBusinessLogicException(UserErrorCode.PasswordTokenIsExpired);

                //user.PasswordToken = null;
                user.PasswordTokenDate = DateTime.UtcNow.AddDays(-1);
                user.Password = CryptographyHelper.MD5(dto.Password);
                UserAPI.UpdateUser(user);

                ts.Complete();
            }
        }
        public UserDto ValidateUserForPasswordReset(PasswordResetDto dto)
        {
            var users = UserAPI.RetrieveUsers(new UserFilter { Name = dto.UserName, SpId = -1 });
            if (users == null || users.Length == 0) ThrowBusinessLogicException(UserErrorCode.UserNameDoesNotExist);

            var userEntity = users[0];
            if (userEntity.PasswordToken != dto.PasswordToken) ThrowBusinessLogicException(UserErrorCode.PasswordTokenDontMatch);
            if (userEntity.PasswordTokenDate < DateTime.UtcNow) ThrowBusinessLogicException(UserErrorCode.PasswordTokenIsExpired); 
            var user = UserTranslator.UserEntity2UserDto(userEntity);
            return user;
        }
        public UserDto ValidateUserForInitPassword(PasswordResetDto dto)
        {
            var users = UserAPI.RetrieveUsers(new UserFilter { Name = dto.UserName, SpId = -1 });
            if (users == null || users.Length == 0) ThrowBusinessLogicException(UserErrorCode.UserNameDoesNotExist);

            var userEntity = users[0];
            if (userEntity.PasswordToken != dto.PasswordToken) ThrowBusinessLogicException(UserErrorCode.PasswordTokenDontMatch);
            if (userEntity.PasswordTokenDate.HasValue && userEntity.PasswordTokenDate != InitPasswordExpirationTime) ThrowBusinessLogicException(UserErrorCode.PasswordTokenIsExpired);
            var user = UserTranslator.UserEntity2UserDto(userEntity);
            return user;
        }

        public UserDto ValidateUserForDemoLogin(PasswordResetDto dto)
        {
            var users = UserAPI.RetrieveUsers(new UserFilter { Name = dto.UserName, SpId = -1 });
            if (users == null || users.Length == 0) ThrowBusinessLogicException(UserErrorCode.DemoUserNameDoesNotExist);

            var userEntity = users[0];
            if (userEntity.PasswordToken != dto.PasswordToken) ThrowBusinessLogicException(UserErrorCode.PasswordTokenDontMatch);
            if (userEntity.PasswordTokenDate < DateTime.UtcNow) ThrowBusinessLogicException(UserErrorCode.PasswordTokenIsExpired); 
            var user = UserTranslator.UserEntity2UserDto(userEntity);
            return user;
        }
        public void RequestForgottenPasswordReset(PasswordResetDto dto)
        {
            ServiceProviderDto sp = null;

            // Put the logical into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateRepeatableRead())
            {
                var users = UserAPI.RetrieveUsers(new UserFilter { Name = dto.UserName, SpId = -1 });
                if (users == null || users.Length == 0) ThrowBusinessLogicException(UserErrorCode.UserNameDoesNotExist);

                var user = users[0];
                if (user.Email != dto.Email) ThrowBusinessLogicException(UserErrorCode.UserNameAndEmailDontMatch);

                user.PasswordToken = Guid.NewGuid().ToString().ToLower();
                user.PasswordTokenDate = DateTime.UtcNow.AddDays(Constant.PASSWORDTOKENEXPIRATIONRELATIVEDAYS);
                UserAPI.UpdateUser(user);

                ValidateSP(out sp, user.SpId);

                ts.Complete();

                var sender = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILFROM];
                //var smtpServer = ConfigurationManager.AppSettings["PasswordMailSmtp"];
                var smtpServer = ConfigHelper.Get(DeploymentConfigKey.SmtpServerIP);
                var senderAccount = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILUSERNAME];
                var senderPwd = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILPASSWORD];

                if (sp.DeployStatus == DeployStatus.Processing)
                {
                    throw new ConcurrentException(Layer.BL,
                        Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderIsBeingSetUpForUsage));
                }
                string webpath = String.Format(ConfigHelper.Get(DeploymentConfigKey.WifAudienceUriTemplate), sp.Domain.ToLower());

                var body = String.Format(
                I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_ResetPasswordEmailBody"),
                        user.RealName,
                        String.Format("{0}index.aspx?u={1}&t={2}&a=resetpwd&amp;lang=" + I18nHelper.LocaleEnumToString(ServiceContext.Language), webpath, user.Name, user.PasswordToken),
                        DateTime.Now.ToString(I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Common_MinuteFormat_1")),
                        this.ProductInfo);

                MailHelper.Send(
                    new[] { user.Email },
                    sender,
                    I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_ResetPasswordTopic"),
                 body,
                    smtpServer,
                    senderAccount, senderPwd, new string[0]);
            }
        }
        public void ResetForgottenPassword(PasswordResetDto dto)
        {
            // Put the logic into a transaction, in case of any concurrency problems.
            using (var ts = TransactionHelper.CreateRepeatableRead())
            {
                var users = UserAPI.RetrieveUsers(new UserFilter { Name = dto.UserName, SpId = -1 });
                if (users == null || users.Length == 0) ThrowBusinessLogicException(UserErrorCode.UserNameIsDeleted);

                var user = users[0];
                if (user.PasswordToken != dto.PasswordToken) ThrowBusinessLogicException(UserErrorCode.PasswordTokenDontMatch);
                if (user.PasswordTokenDate < DateTime.UtcNow) ThrowBusinessLogicException(UserErrorCode.PasswordTokenIsExpired);
                //user.PasswordToken = null;
                user.PasswordTokenDate = DateTime.UtcNow.AddDays(-1);
                user.Password = CryptographyHelper.MD5(dto.Password);
                UserAPI.UpdateUser(user);

                ts.Complete();
            }
        }

        public void DeleteUserEntity(long userId)
        {
            UserAPI.DeleteUser(new UserFilter { UserIds = new[] { userId } });
        }
        public void CreateUserCustomer(UserCustomerEntity[] entities)
        {
            UserAPI.CreateUserCustomer(entities);
        }

        public void DeleteUserCustomer(UserCustomerFilter filter)
        {
            UserAPI.DeleteUserCustomer(filter);
        }

        public UserEntity[] RetrieveUserEntitiesByFilter(UserFilter filter)
        {
            if (filter.UserIds != null) filter.DemoStatus = null;
            if (filter.SpId == 0) filter.SpId = ServiceContext.CurrentUser.SPId;
            return UserAPI.RetrieveUsers(filter);
        }

        public UserDto GetUserById(long userId)
        {
            var userEntities = GetUsersByFilter(new UserFilterDto { UserIds = new[] { userId }, DemoStatus = null, SpId = -1 });
            return userEntities.Length > 0 ? userEntities[0] : null;
        }

        public UserDto[] GetUsersByFilter(UserFilterDto filter)
        {
            var filterEntity = UserTranslator.UserFilterDto2UserFilter(filter);
            if (filterEntity.UserIds != null && filterEntity.UserIds.Length < 2) { filterEntity.DemoStatus = null; }

            if (filter.CustomerId.HasValue)
            {
                var wholeCustomers = filterEntity.CustomerIds = new long[] { filter.CustomerId.Value, 0 };
            }

            if (filterEntity.SpId == 0) filterEntity.SpId = ServiceContext.CurrentUser.SPId;
            var entities = UserAPI.RetrieveUsers(filterEntity);
            var dtos = entities.Select(UserTranslator.UserEntity2UserDto).ToArray();

            foreach (var userDto in dtos)
            {
                if (userDto.SpId == -1)
                {
                    userDto.UserType = BLConfiguratgionKey.PLATFORMUSERTYPE;
                }
                else
                {
                    var roleFilter = new RoleFilter() { SpId = ServiceContext.CurrentUser.SPId };
                    if (filter.UserType.HasValue) roleFilter.RoleIds = new[] { filter.UserType.Value };
                    roleFilter.UserIds = new[] { userDto.Id.Value };

                    var roles = AccessControlBL.RetrieveRolesByFilter(roleFilter);
                    if (roles != null && roles.Length > 0)
                    {
                        userDto.UserType = roles[0].Id.Value;
                        userDto.UserTypeName = roles[0].Name;
                    }

                    var customers = UserAPI.RetrieveUserCustomers(new UserCustomerFilter { UserIds = new[] { userDto.Id.Value } });
                    userDto.CustomerIds = customers.Select(p => p.CustomerId).ToArray();
                }
            }

            return dtos;
        }

        public UserCustomerEntity[] RetrieveUserCustomers(UserCustomerFilter filter)
        {
            return UserAPI.RetrieveUserCustomers(filter);
        }

        public UserDto CloneDemoUser(String email)
        {
            var usernameCfg = ConfigurationKey.DEMOTEMPLATE;
            var username = ConfigHelper.Get(usernameCfg);
            var users = UserAPI.RetrieveUsers(new UserFilter { Name = username, SpId = -1 });
            if (users == null || users.Length == 0) return null;
            var user = users[0];
            var oldUserId = user.Id.Value;

            var newusername = Guid.NewGuid().ToString().Replace("-", "");
            ServiceContext.SetServiceContext(Guid.NewGuid(), new Infrastructure.Utils.User { Name = newusername });

            #region clone user

            user.Name = newusername;
            user.Id = null;
            user.Password = CryptographyHelper.MD5(Constant.DEMOUSERPASSWORD);
            user.PasswordToken = Guid.NewGuid().ToString();
            user.PasswordTokenDate = DateTime.UtcNow.AddDays(Constant.PASSWORDTOKENEXPIRATIONRELATIVEDAYS);
            user.UpdateTime = System.DateTime.UtcNow;
            user.UpdateUser = ServiceContext.CurrentUser.Name;
            user.DemoStatus = 1;
            user.Email = email;
            user.Id = UserAPI.CreateUser(user);

            #endregion

            #region clone usercustomer

            var usercustomers = UserAPI.RetrieveUserCustomers(new UserCustomerFilter { UserIds = new[] { oldUserId } });

            foreach (var item in usercustomers)
            {
                item.Id = null;
                item.UserId = user.Id.Value;
                item.UpdateTime = System.DateTime.Now;
                item.UpdateUser = ServiceContext.CurrentUser.Name;
            }

            if (usercustomers.Length > 0)
            {
                UserAPI.CreateUserCustomer(usercustomers);
            }

            #endregion

            #region clone userrole

            var userroles = new List<UserRoleEntity>();
            var roles = AccessControlBL.RetrieveRolesByFilter(new RoleFilter { UserIds = new[] { oldUserId }, SpId = -1 });
            foreach (var item in roles)
            {
                userroles.Add(new UserRoleEntity
                {
                    UserId = user.Id.Value,
                    RoleId = item.Id.Value,
                });
            }
            if (userroles.Count > 0)
            {
                UserAPI.CreateUserRole(userroles.ToArray());
            }

            #endregion

            #region clone dataprivilege

            var dataPris = AccessControlBL.RetrieveDataPrivilegeEntities(new DataPrivilegeFilter { UserIds = new[] { oldUserId } });
            foreach (var item in dataPris)
            {
                item.Id = null;
                item.UserId = user.Id.Value;
                item.UpdateTime = System.DateTime.Now;
                item.UpdateUser = ServiceContext.CurrentUser.Name;
            }

            if (dataPris.Length > 0)
            {
                AccessControlBL.CreateDataPrivilegeEntities(dataPris);
            }

            #endregion

            #region clone dashboard & widget

            //todo:add extensions
            // ShareInfoBL.CloneDashboards(oldUserId, user.Id.Value);

            var dict = new Dictionary<string, string>();
            dict.Add("oldUserId", oldUserId.ToString());
            dict.Add("newUserId", user.Id.Value.ToString());
            ActionExcusor.Fire("UserService.CloneDemoUser", true, PositionType.After, dict);

            #endregion

            var rtnuser = GetUserById(user.Id.Value);

            if (rtnuser.SpId > 0)
            {
                var roleFilter = new RoleFilter();
                roleFilter.UserIds = new[] { rtnuser.Id.Value };

                var rtnRoles = AccessControlBL.RetrieveRolesByFilter(roleFilter);
                if (rtnRoles != null && rtnRoles.Length > 0) rtnuser.UserType = roles[0].Id.Value;

                var customers = UserAPI.RetrieveUserCustomers(new UserCustomerFilter { UserIds = new[] { rtnuser.Id.Value } });
                rtnuser.CustomerIds = customers.Select(p => p.CustomerId).ToArray();
            }
            else
            {
                rtnuser.UserType = BLConfiguratgionKey.PLATFORMUSERTYPE;
            }

            var sender = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILFROM];
            //var smtpServer = ConfigurationManager.AppSettings["PasswordMailSmtp"];
            var smtpServer = ConfigHelper.Get(DeploymentConfigKey.SmtpServerIP);
            var senderAccount = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILUSERNAME];
            var senderPwd = ConfigurationManager.AppSettings[BLConfiguratgionKey.PASSWORDMAILPASSWORD];

            var sps = ServiceProviderBL.GetServiceProviders(new ServiceProviderFilter { Ids = new[] { user.SpId } });
            var sp = sps[0];

            //  string topic = string.Format("欢迎试用{0}", ProductInfo.PRODUCTNAME);
            //   string sysName = I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "BLCOMMON_ProductInfo");
            string topic = string.Format(
              I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_CloneDemoUserEmailHeader"),
             this.ProductInfo);

            string webpath = String.Format(ConfigHelper.Get(DeploymentConfigKey.WifAudienceUriTemplate), sp.Domain.ToLower());
            string body = String.Format(I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_CloneDemoUserEmailBody"),
                   user.RealName,
                   String.Format("{0}index.aspx?u={1}&t={2}&a=demologin&amp;lang=" + I18nHelper.LocaleEnumToString(ServiceContext.Language), webpath, user.Name, user.PasswordToken),
                   DateTime.Now.ToString(I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Common_MinuteFormat_1")),
                   this.ProductInfo);

            //var webpath = sp.WebPath;

            // If user email is blank, the request must be from BLUES,  2014.1.15
            //
            if (!String.IsNullOrWhiteSpace(user.Email))
            {
                MailHelper.Send(
                    new[] { user.Email }, sender, topic, body, smtpServer,
                    senderAccount, senderPwd, new string[0]);
            }

            return rtnuser;
        }

        private void CreateDefaultData(long userId, long spid)
        {
            var updateuser = ServiceContext.CurrentUser.Name;
            var updateTime = DateTime.UtcNow;

            var r1 = RoleAPI.CreateRole(new RoleEntity { Name = "服务商管理员", Comment = "", UpdateUser = updateuser, UpdateTime = updateTime, SpId = spid });
            var r2 = RoleAPI.CreateRole(new RoleEntity { Name = "客户管理员", Comment = "", UpdateUser = updateuser, UpdateTime = updateTime, SpId = spid });
            var r3 = RoleAPI.CreateRole(new RoleEntity { Name = "咨询人员", Comment = "", UpdateUser = updateuser, UpdateTime = updateTime, SpId = spid });
            var r4 = RoleAPI.CreateRole(new RoleEntity { Name = "工程人员", Comment = "", UpdateUser = updateuser, UpdateTime = updateTime, SpId = spid });
            var r5 = RoleAPI.CreateRole(new RoleEntity { Name = "商务人员", Comment = "", UpdateUser = updateuser, UpdateTime = updateTime, SpId = spid });

            #region RolePrivilege
            RolePrivilegeAPI.CreateRolePrivilege(new[] 
            {
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1200", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1201", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1202", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1203", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1206", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1207", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1208", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1210", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1217", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1211", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1213", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1214", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1215", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r1, PrivilegeCode="1216", UpdateUser = updateuser, UpdateTime = updateTime },

                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1200", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1201", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1202", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1203", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1207", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1208", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1210", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1217", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1211", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1212", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1213", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1214", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1215", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r2, PrivilegeCode="1216", UpdateUser = updateuser, UpdateTime = updateTime },
                
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1200", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1201", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1202", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1203", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1205", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1207", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1208", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1210", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1217", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1211", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1213", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1214", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1215", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r3, PrivilegeCode="1216", UpdateUser = updateuser, UpdateTime = updateTime },

                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1100", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1101", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1102", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1200", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1201", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1202", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1203", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1211", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1213", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1214", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1215", UpdateUser = updateuser, UpdateTime = updateTime },
                new RolePrivilegeEntity{ RoleId=r4, PrivilegeCode="1216", UpdateUser = updateuser, UpdateTime = updateTime },

                new RolePrivilegeEntity{ RoleId=r5, PrivilegeCode="1211", UpdateUser = updateuser, UpdateTime = updateTime },
            });
            #endregion

            UserAPI.CreateUserCustomer(new[] { new UserCustomerEntity { UserId = userId, CustomerId = 0, UpdateTime = updateTime, UpdateUser = updateuser } });
            //UserAPI.CreateUserRole(new[] { new UserRoleEntity { UserId = userId, RoleId = r1 } });
        }

        public void UpdateUser(UserEntity user)
        {
            UserAPI.UpdateUser(user);
        }

        #region Validate

        private bool ValidateRealName(UserDto dto)
        {
            if (!Regex.IsMatch(dto.RealName, ConstantValue.PERSONNAMEREGEX))
            {
                ThrowParameterException(UserErrorCode.UserRealNameFormatNotCorrect);
            }
            if (dto.RealName.Length > ConstantValue.PERSONNAMELENGTHLIMITATION)
            {
                ThrowParameterException(UserErrorCode.UserRealNameLengthNotCorrect);
            }
            return true;
        }
        public bool ValidateName(UserDto dto)
        {
            if (!Regex.IsMatch(dto.Name, ConstantValue.USERIDREGEX))
            {
                ThrowParameterException(UserErrorCode.UserIdFormatNotCorrect);
            }
            if (dto.Name.Length > ConstantValue.USERIDMAXLENGTH)
            {
                ThrowParameterException(UserErrorCode.UserIdLengthNotCorrect);
            }

            // validate meta data user
            var users = UserAPI.RetrieveUsers(new UserFilter { Name = dto.Name, SpId = -1 });
            if (users != null && users.Length > 0)
            {
                if (!dto.Id.HasValue)
                {
                    return false;
                }
                else if (dto.Id.Value != users[0].Id.Value || (dto.Id.Value == users[0].Id.Value) && ServiceContext.CurrentUser.SPId != BLConfiguratgionKey.PLATFORMSPID)
                {
                    return false;
                }
            }

            //validate sps
            var allSps = ServiceProviderBL.GetServiceProviders(new ServiceProviderFilter
            {
                StatusFilter = new StatusFilter
                {
                    Statuses = new[] { EntityStatus.Deleted },
                    ExcludeStatus = true
                }
            });

            foreach (var sp in allSps)
            {
                if (sp.UserName == dto.Name)
                {
                    return false;
                }

                if (sp.DeployStatus == DeployStatus.Finished)
                {
                    // TODO: CONN
                    users = UserAPI.RetrieveUsers(new UserFilter { Name = dto.Name, SpId = -1 });

                    if (users != null && users.Length > 0)
                    {
                        if (!dto.Id.HasValue)
                        {
                            return false;
                        }
                        else if (dto.Id.Value != users[0].Id.Value || (dto.Id.Value == users[0].Id.Value) && ServiceContext.CurrentUser.SPId != sp.Id.Value)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        public UserDto ValidateLogin(string userName, string password)
        {
            UserDto currentUser = null;

            var users = UserAPI.RetrieveUsers(new UserFilter { Name = userName, DemoStatus = null, SpId = -1 });

            if (users == null || users.Length == 0 || users[0] == null) return null;

            var user = UserTranslator.UserEntity2UserDto(users[0]);
 

            var roleFilter = new RoleFilter();
            roleFilter.UserIds = new[] { user.Id.Value };

            var roles = AccessControlBL.RetrieveRolesByFilter(roleFilter);
            if (roles != null && roles.Length > 0) user.UserType = roles[0].Id.Value;

            var customers = UserAPI.RetrieveUserCustomers(new UserCustomerFilter { UserIds = new[] { user.Id.Value } });
            user.CustomerIds = customers.Select(p => p.CustomerId).ToArray();


            if (user.SpId != -1)
            {
                var allSps = ServiceProviderBL.GetServiceProviders(new ServiceProviderFilter
                {
                    StatusFilter = new StatusFilter { },
                    Ids = new[] { user.SpId },
                });

                if (allSps.Length == 0)
                {
                    return null;
                }
                var sp = allSps[0];
                if (sp.Status == EntityStatus.Deleted)
                {
                    return null;
                }
                if (sp.DeployStatus == DeployStatus.Processing)
                {
                    return null;
                }

                user.SpStatus = sp.Status;

                if (sp.Status == EntityStatus.Inactive)
                {
                    currentUser = user;
                    return null;
                }
                if (user.Password.ToLower() == CryptographyHelper.MD5(password).ToLower() && sp.Status == EntityStatus.Active)
                {
                    currentUser = user;
                    return user;
                }
                else
                {
                    currentUser = user;
                    return null;
                }
            }
            else
            {
                if (user.Password.ToLower() == CryptographyHelper.MD5(password).ToLower())
                {
                    currentUser = user;
                    return user;
                }
                else
                {
                    currentUser = user;
                    return null;
                }
            }
            
        }
        public UserDto ValidateSpLogin(string spdomain, string userName, string password)
        {
            UserDto currentUser = null;

            var users = UserAPI.RetrieveUsers(new UserFilter { Name = userName, DemoStatus = null, SpId = -1 });

            if (users == null || users.Length == 0 || users[0] == null) return null;

            var user = UserTranslator.UserEntity2UserDto(users[0]);

            var roleFilter = new RoleFilter();
            roleFilter.UserIds = new[] { user.Id.Value };

            var roles = AccessControlBL.RetrieveRolesByFilter(roleFilter);
            if (roles != null && roles.Length > 0) user.UserType = roles[0].Id.Value;

            var customers = UserAPI.RetrieveUserCustomers(new UserCustomerFilter { UserIds = new[] { user.Id.Value } });
            user.CustomerIds = customers.Select(p => p.CustomerId).ToArray();


            if (user.SpId != -1)
            {
                var allSps = ServiceProviderBL.GetServiceProviders(new ServiceProviderFilter
                {
                    StatusFilter = new StatusFilter { },
                    Ids = new[] { user.SpId },
                });
                if (allSps.Length == 0)
                {
                    return null;
                }
                var sp = allSps[0];
                if (!String.IsNullOrEmpty(sp.Domain) && spdomain.ToLower().IndexOf(sp.Domain.ToLower()) < 0)
                {
                    return null;
                }
                if (sp.Status == EntityStatus.Deleted)
                {
                    return null;
                }
                if (sp.DeployStatus == DeployStatus.Processing)
                {
                    return null;
                }

                user.SpStatus = sp.Status;

                if (sp.Status == EntityStatus.Inactive)
                {
                    currentUser = user;
                    return null;
                }
                if (user.Password.ToLower() == CryptographyHelper.MD5(password).ToLower() && sp.Status == EntityStatus.Active)
                {
                    currentUser = user;
                    return user;
                }
                else
                {
                    currentUser = user;
                    return user;
                }
            }
            else
            {
                var platformDomain = ConfigHelper.Get(DeploymentConfigKey.LoginPlatformDomain);
                if (!String.IsNullOrEmpty(platformDomain) && spdomain.ToLower() != platformDomain)
                {
                    return null;
                }

                if (user.Password.ToLower() == CryptographyHelper.MD5(password).ToLower())
                {
                    currentUser = user;
                    return user;
                }
                else
                {
                    currentUser = user;
                    return null;
                }
            }
        }

        public void DeleteUserRole(RoleFilter filter)
        {
            UserAPI.DeleteUserRole(filter);
        }

        #endregion

        private bool ValidateSP(out ServiceProviderDto sp, long spId)
        {
            var sps = ServiceProviderBL.GetServiceProviders(new ServiceProviderFilter { Ids = new[] { spId } });

            if (sps.Length == 0)
            {
                throw new ConcurrentException(Layer.BL,
                    Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderHasBeenDeleted));
            }
            sp = sps[0];
            //if (sp.DeployStatus == DeployStatus.Processing)
            //{
            //    throw new ConcurrentException(Layer.BL,
            //        Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderIsBeingSetUpForUsage));
            //}
            if (sp.Status == EntityStatus.Inactive)
            {
                throw new BusinessLogicException(Layer.BL,
                    Module.ServiceProvider, Convert.ToInt32(ServiceProviderErrorCode.ServiceProviderIsPaused));
            }
            return true;
        }

        #region Exception

        protected void ThrowConcurrentException(UserErrorCode error)
        {
            throw new ConcurrentException(Layer.BL, Module.User, Convert.ToInt32(error));
        }
        protected void ThrowBusinessLogicException(UserErrorCode error)
        {
            throw new BusinessLogicException(Layer.BL, Module.User, Convert.ToInt32(error));
        }
        protected void ThrowInputException(UserErrorCode error)
        {
            throw new InputException(Layer.BL, Module.User, Convert.ToInt32(error));
        }
        protected void ThrowParameterException(UserErrorCode error)
        {
            throw new ParameterException(Layer.BL, Module.User, Convert.ToInt32(error));
        }
        protected void ThrowDataAuthExcepton()
        {
            throw new DataAuthorizationException(Layer.BL, Module.AccessControl, Convert.ToInt32(ErrorCode.NoDataPrivilege));
        }

        #endregion

        public static class Util
        {
            private static char[] constant =
                {
                    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                };

            public static string GenerateRandom()
            {
                var len = DateTime.Now.Ticks % 3 + 6;

                var newRandom = new StringBuilder(62);
                Random rd = new Random();
                for (int i = 0; i < len; i++)
                {
                    newRandom.Append(constant[rd.Next(62)]);
                }
                newRandom.Append(constant[rd.Next(52)]);
                newRandom.Append(rd.Next(10));
                return newRandom.ToString();
            }
        }

        #region ContactUs
        public void SendContactUsMail(ContactUsDto dto)
        {
            // get mail to
            string[] mailTo = ConfigurationManager.AppSettings[BLConfiguratgionKey.CONTACTUSMAILTO].Split(ConstantValue.Delimiter);

            #region attachment
            var workbook = SpreadsheetGear.Factory.GetWorkbook();
            var cells = workbook.Worksheets["Sheet1"].Cells;

            cells["A1"].Value = I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "WEBAPI_UserName");
            cells["B1"].Value = dto.Name;
            cells["A2"].Value = I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "WEBAPI_Telephone");
            //cells["A2"].Value = "电话"; 
            cells["B2"].Value = dto.Telephone;
            //cells["A3"].Value = "公司";
            cells["A3"].Value = I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "WEBAPI_Company");
            cells["B3"].Value = dto.CustomerName;
            //cells["A4"].Value = "职位"; 
            cells["A4"].Value = I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "WEBAPI_Title");
            cells["B4"].Value = dto.Title;
            //cells["A5"].Value = "描述";
            cells["A5"].Value = I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "WEBAPI_Description");

            cells["B5"].Value = dto.Comment;

            var attachementName = new StringBuilder(dto.CustomerName).Append("_")
                .Append(dto.Name).Append("_")
                .Append(DateTime.UtcNow.ToString("yyyyMMddHHmm")).Append(".xls").ToString();
            #endregion

            #region mail body
            var attachment = workbook.SaveToMemory(FileFormat.Excel8);

            //  var title = string.IsNullOrEmpty(dto.Title) ? string.Empty : string.Format("职位：{0}\r\n", dto.Title);// +Environment.NewLine;
            var title = string.IsNullOrEmpty(dto.Title) ? string.Empty : string.Format(I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "WEBAPI_MailContactTitle"), dto.Title);// +Environment.NewLine;
            //  var comment = string.IsNullOrEmpty(dto.Comment) ? string.Empty : string.Format("“联系我们”信息描述：{0}\r\n" + dto.Comment);// +Environment.NewLine;

            var comment = string.IsNullOrEmpty(dto.Comment) ? string.Empty : string.Format(I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "WEBAPI_MailContactComment"), dto.Comment);// +Environment.NewLine;

            var currentUsers = this.UserAPI.RetrieveUsers(new UserFilter()
            {
                UserIds = new long[] { ServiceContext.CurrentUser.Id },
                SpId = -1
            });

            if (currentUsers.Length == 0)
            {
                throw new REMException(ErrorType.Custom, Layer.BL, Module.User, Convert.ToInt32(UserErrorCode.UserDoesNotExist));
            }
            //       string sysName = I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "BLCOMMON_ProductInfo");

            var body = string.Format(
               I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "WEBAPI_MailContactUsBody"),
               dto.Name,
               dto.Telephone,
               currentUsers[0].Email,
               dto.CustomerName,
               title,
               comment,
             this.ProductInfo);


            var topic = string.Format(I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "WEBAPI_MailContactUsTopic"), this.ProductInfo);
            #endregion

            //send mail
            MailHelper.Send(mailTo,
                            ConfigurationManager.AppSettings[BLConfiguratgionKey.CONTACTUSMAILFROM],
                            topic,
                            body,
                            ConfigHelper.Get(DeploymentConfigKey.SmtpServerIP),
                            ConfigurationManager.AppSettings[BLConfiguratgionKey.CONTACTUSMAILUSERNAME],
                            ConfigurationManager.AppSettings[BLConfiguratgionKey.CONTACTUSMAILPASSWORD],
                            new Dictionary<string, byte[]>() { { attachementName, attachment } }
            );
        }
        #endregion
    }
}
