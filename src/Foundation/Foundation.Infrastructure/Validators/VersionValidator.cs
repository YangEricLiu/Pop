using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace SE.DSP.Foundation.Infrastructure.Validators
{
    public class VersionValidator : ValidatorBase<long>
    {
        protected override void DoValidate(long objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate < 1)
            {
                string errorCode = MessageTemplate + ErrorCode.RANGEERROR;

                LogValidationResult(validationResults, errorCode, currentTarget, key);
            }
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate == null)
                return;

            DoValidate(Convert.ToInt64(objectToValidate), currentTarget, key, validationResults);
        }

    }

    public class VersionValidatorAttribute : ValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new VersionValidator();
        }
    }
}