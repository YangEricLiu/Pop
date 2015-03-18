using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using SE.DSP.Foundation.Infrastructure.Constant;


namespace SE.DSP.Foundation.Infrastructure.Validators
{
    public class YearRangeValidator : ValidatorBase<int>
    {
        protected override void DoValidate(int objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate > ConstantValue.EFFECTIVEUPPERYEARBOUND || objectToValidate < ConstantValue.EFFECTIVELOWERYEARBOUND)
            {
                string errorCode = MessageTemplate + ErrorCode.RANGEERROR;

                LogValidationResult(validationResults, errorCode, currentTarget, key);
            }
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate == null)
                return;

            DoValidate(Convert.ToInt32(objectToValidate), currentTarget, key, validationResults);
        }

    }

    public class YearRangeValidatorAttribute : ValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new YearRangeValidator();
        }
    }
}