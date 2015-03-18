using System;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using SE.DSP.Foundation.Infrastructure.Constant;


namespace SE.DSP.Foundation.Infrastructure.Validators
{
    public class NameValidator : ValidatorBase<string>
    {
        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            string errorCode = null;

            if (string.IsNullOrEmpty(objectToValidate))
            {
                errorCode = MessageTemplate + ErrorCode.NULLERROR;
            }
            else
            {
                objectToValidate = objectToValidate.Trim();

                if (objectToValidate.Length > ConstantValue.NAMELENGTHLIMITATION)
                {
                    errorCode = MessageTemplate + ErrorCode.LENGTHERROR;
                }
                else if (!Regex.IsMatch(objectToValidate, ConstantValue.NAMEREGEX))
                {
                    errorCode = MessageTemplate + ErrorCode.FORMATERROR;
                }
            }

            if (!string.IsNullOrEmpty(errorCode))
            {
                LogValidationResult(validationResults, errorCode, currentTarget, key);
            }
        }
    }

    public class NameValidatorAttribute : ValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new NameValidator();
        }
    }
}