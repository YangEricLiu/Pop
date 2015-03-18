using System;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using SE.DSP.Foundation.Infrastructure.Constant;
using SE.DSP.Foundation.Infrastructure.Enumerations;


namespace SE.DSP.Foundation.Infrastructure.Validators
{
    public class CommentValidator : ValidatorBase<string>
    {
        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (!String.IsNullOrEmpty(objectToValidate))
            {
                objectToValidate = objectToValidate.Trim();

                if (!String.IsNullOrEmpty(objectToValidate))
                {
                    string errorCode = null;

                    if (objectToValidate.Length > ConstantValue.COMMENTLENGTHLIMITATION)
                    {
                        errorCode = MessageTemplate + ErrorCode.LENGTHERROR;
                    }
                    else if (!Regex.IsMatch(objectToValidate, ConstantValue.COMMENTREGEX))
                    {
                        errorCode = MessageTemplate + ErrorCode.FORMATERROR;
                    }

                    if (!string.IsNullOrEmpty(errorCode))
                    {
                        LogValidationResult(validationResults, errorCode, currentTarget, key);
                    }
                }
            }
        }
    }

    public class CommentValidatorAttribute : ValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new CommentValidator();
        }
    }
}