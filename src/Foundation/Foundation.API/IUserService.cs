using SE.DSP.Foundation.Infrastructure.BaseClass;

using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure;

using SE.DSP.Foundation.Infrastructure.BE.Entities;
using System.ServiceModel;

namespace SE.DSP.Foundation.API
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        UserDto CreateUser(UserDto dto);

        [OperationContract]
        UserDto ModifyUser(UserDto dto);

        [OperationContract]
        UserDto ModifyProfile(UserDto dto);

        [OperationContract]
        void DeleteUser(DtoBase dto);

        [OperationContract]
        void DeleteUserEntity(long id);

        [OperationContract]
        UserDto GetUserById(long userId);

        [OperationContract]
        UserDto[] GetUsersByFilter(UserFilterDto filter);

        [OperationContract]
        UserEntity[] RetrieveUserEntitiesByFilter(UserFilter filter);

        [OperationContract]
        PasswordDto ResetPassword(PasswordDto dto);

        [OperationContract]
        DtoBase SendInitPassword(long userId);

        [OperationContract]
        void SendSPInitPassword(long spId);

        [OperationContract]
        bool ValidateName(UserDto dto);

        [OperationContract]
        UserDto ValidateLogin(string userName, string password);

        [OperationContract]
        UserDto ValidateSpLogin(string spdomain, string userName, string password);

        [OperationContract]
        UserDto ValidateUserForPasswordReset(PasswordResetDto dto);

        [OperationContract]
        UserDto ValidateUserForInitPassword(PasswordResetDto dto);

        [OperationContract]
        UserDto ValidateUserForDemoLogin(PasswordResetDto dto);

        [OperationContract]
        void SetInitPassword(PasswordResetDto dto);

        [OperationContract]
        void RequestForgottenPasswordReset(PasswordResetDto dto);

        [OperationContract]
        void ResetForgottenPassword(PasswordResetDto dto);

        [OperationContract]
        void CreateUserCustomer(UserCustomerEntity[] entities);

        [OperationContract]
        void DeleteUserCustomer(UserCustomerFilter filter);

        [OperationContract]
        UserCustomerEntity[] RetrieveUserCustomers(UserCustomerFilter filter);

        [OperationContract]
        UserDto CloneDemoUser(string email);

        [OperationContract]
        void SendContactUsMail(ContactUsDto dto);

        [OperationContract]
        void DeleteUserRole(RoleFilter filter);

        [OperationContract]
        void UpdateUser(UserEntity user);
    }
}
