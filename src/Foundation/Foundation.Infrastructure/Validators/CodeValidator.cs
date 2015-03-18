using System;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using SE.DSP.Foundation.Infrastructure.Constant;
using SE.DSP.Foundation.Infrastructure.Enumerations;


namespace SE.DSP.Foundation.Infrastructure.Validators
{
    public class CodeValidator : ValidatorBase<string>
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

                if (objectToValidate.Length > ConstantValue.CODELENGTHLIMITATION)
                {
                    errorCode = MessageTemplate + ErrorCode.LENGTHERROR;
                }
                else if (!Regex.IsMatch(objectToValidate, ConstantValue.CODEREGEX))
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

    public class CodeValidatorAttribute : ValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new CodeValidator();
        }
    }
}