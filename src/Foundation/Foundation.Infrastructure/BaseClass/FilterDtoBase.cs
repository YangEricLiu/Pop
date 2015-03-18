using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using SE.DSP.Foundation.Infrastructure.Validators;


namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    public class FilterDtoBase
    {
        /// <summary>
        /// CustomerId
        /// </summary>
        [RangeValidator(1, RangeBoundaryType.Inclusive, long.MaxValue, RangeBoundaryType.Inclusive, Ruleset = "create", MessageTemplate = FieldCode.AreaDimensionId)]
        [RangeValidator(1, RangeBoundaryType.Inclusive, long.MaxValue, RangeBoundaryType.Inclusive, Ruleset = "update", MessageTemplate = FieldCode.AreaDimensionId)]
        public long CustomerId { get; set; }
        ///// <summary>
        ///// IgnoreDataAuth
        ///// </summary>
        //public bool? IgnoreDataAuth { get; set; }

    }
}
