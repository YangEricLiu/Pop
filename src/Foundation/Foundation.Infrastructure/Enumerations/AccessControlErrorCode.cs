/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: AccessControlErrorCode.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Error code for access control module
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// 
    /// </summary>
    public enum AccessControlErrorCode
    {
        DataPrivilegeIsExpired = 001,
        RoleIsExpired = 002,
        //UserTypePrivilegeExceedsLimit = 003,
        //UserTypePrivilegeIsEmpty = 004,
        ParameterIsIncorrect = 005,
        UserDoesNotExist = 006,
        SomeCustomersAreDeleted = 007,
        SomeHierarchiesAreDeleted = 018,
        RoleDoesNotExist = 019,
        RoleNameIsDuplicated = 010,
        UserExistsAnagistDeletion = 011,
        HierarchyHasBeenDeleted = 021,
    }
}
