using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Validators;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Utils;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UserDto
    {
        [IdValidator(Ruleset = "update", MessageTemplate = FieldCode.ID)]
        public long? Id { get; set; }
        public String RealName { get; set; }
        public long UserType { get; set; }
        public String UserTypeName { get; set; }
        //[IgnoreNulls]
        //[PasswordValidator(Ruleset = "create", MessageTemplate = FieldCode.Password)]
        //[PasswordValidator(Ruleset = "update", MessageTemplate = FieldCode.Password)]
        public String Password { get; set; }
        //public long[] RoleIds { get; set; }
        public long[] CustomerIds { get; set; }
        public UserTitle Title { get; set; }
        public String Telephone { get; set; }
        //[RegexValidator(Schneider.REM.Common.ConstantValue.PASSWORDREGEX, Ruleset = "update", MessageTemplate = FieldCode.Email)]
        public String Email { get; set; }
        public String Comment { get; set; }
        public String Name { get; set; }
        public int DemoStatus { get; set; }
        public long SpId { get; set; }
        public EntityStatus SpStatus { get; set; }

        [VersionValidator(Ruleset = "update", MessageTemplate = FieldCode.VERSION)]
        public long? Version { get; set; }

        public override string ToString()
        {
            return new StringBuilder("Id: ").Append(this.Id).Append(", Name: ").Append(this.Name).ToString();
        }

        public static String GetTitle(UserTitle title)
        {
            switch (title)
            {
                case UserTitle.EEConsultant:
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_EEConsultant");
                case UserTitle.Technician:
                    //return "技术人员";
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_Technician");
                case UserTitle.CustomerAdmin:
                    //return "客户管理员";
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_CustomerAdmin");
                case UserTitle.PlatformAdmin:
                    //return "平台管理员";
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_PlatformAdmin");
                case UserTitle.EnergyManager:
                   // return "能源经理";
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_EnergyManager");
                case UserTitle.EnergyEngineer:
                    //return "能源工程师";
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_EnergyEngineer");
                case UserTitle.DepartmentManager:
                   // return "部门经理";
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_DepartmentManager");
                case UserTitle.CEO:
                   // return "管理层";
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_CEO");
                case UserTitle.BusinessPersonnel:
                   // return "业务人员";
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_BusinessPersonnal");
                case UserTitle.Saleman:
                   // return "销售人员";
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_Saleman");
                case UserTitle.ServiceProviderAdmin:
                    //return "服务商管理员";
                    return I18nHelper.GetValue(ServiceContext.Language, I18nResourceType.App, "Customer_ServiceProviderAdmin");
                default:
                    return "";
            }
        }
    }
}
