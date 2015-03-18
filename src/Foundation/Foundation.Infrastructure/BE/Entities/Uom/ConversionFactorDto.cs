using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class ConversionFactorDto
    {
        public UomConversionDto[] UomConversionFactors { get; set; }
        public StandardCoalConversionDto[] StandardCoalConversionFactors { get; set; }
    }
}
