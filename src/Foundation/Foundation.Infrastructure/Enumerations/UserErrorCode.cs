/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: UserErrorCode.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Error code for user module
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    public enum UserErrorCode
    {
        UserNameIsDuplicated = 001,
        UserIsExpired = 002,
        PasswordIsIncorrect = 003,
        CustomerInfoIsIncorrect = 004,
        CustomerListHasExpiredItems = 005,
        DefualtPlatformAdministratorCantBeDeleted = 006,
        CustomerAdministratorExists = 007,
        UserDoesNotExist = 008,
        UserCannotDeleteOneself = 009,
        UserCannotGeneratePasswordForOneself = 010,
        UserCanOnlyResetPasswordForOneself = 011,
        UserCannotModifyOthersProfile = 012,
        UserRealNameFormatNotCorrect = 013,
        UserRealNameLengthNotCorrect = 014,
        UserIdFormatNotCorrect = 015,
        UserIdLengthNotCorrect = 016,
        //PasswordDoesNotMatchRule = 012,

        FeedbackAttachmentSizeOverLimitation = 050, //KB
        FeedbackAttachmentFormatIsUnsupported = 051,
        FeedbackMailFailed = 052,
        FeedbackProblemDescriptionSizeOverLimitation = 053,
        FeedbackProblemDescriptionFormatIsNotCorrect = 054,

        UserNameDoesNotExist = 100,
        UserNameAndEmailDontMatch = 101,
        PasswordTokenDontMatch = 102,
        PasswordTokenIsExpired = 103,
        ActionIsProhibited = 104,
        //登录失败，您的用户名暂时无法使用
        ServiceProviderIsPaused = 105,
        ServiceProviderNotExist = 106,

        UserNameIsDeleted = 107,

        DemoUserNameDoesNotExist = 109,
    }
}
