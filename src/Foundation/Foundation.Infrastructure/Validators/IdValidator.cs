using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace SE.DSP.Foundation.Infrastructure.Validators
{
    public class IdValidator : ValidatorBase<long>
    {
        private bool IsAllowZero { get; set; }

        public IdValidator(bool isAllowZero)
        {
            this.IsAllowZero = isAllowZero;
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate == null)
                return;

            DoValidate(Convert.ToInt64(objectToValidate), currentTarget, key, validationResults); 
        }

        protected override void DoValidate(long objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if ( objectToValidate< (this.IsAllowZero ? 0 : 1))
            {
                string errorCode = MessageTemplate + ErrorCode.RANGEERROR;

                LogValidationResult(validationResults, errorCode, currentTarget, key);
            }
        }
    }

    public class IdValidatorAttribute : ValidatorAttribute
    {
        public bool IsAllowZero { get; set; }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new IdValidator(IsAllowZero);
        }
    }
}