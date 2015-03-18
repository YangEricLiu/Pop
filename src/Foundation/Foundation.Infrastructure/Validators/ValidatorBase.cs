using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace SE.DSP.Foundation.Infrastructure.Validators
{
    public abstract class ValidatorBase<T> : Validator<T>
    {
        public ValidatorBase()
            : base(String.Empty, String.Empty)
        {
        }

        protected override string DefaultMessageTemplate
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}