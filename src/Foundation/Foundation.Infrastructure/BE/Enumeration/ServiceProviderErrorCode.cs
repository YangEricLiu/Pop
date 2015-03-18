using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.BE.Enumeration
{
    public enum ServiceProviderErrorCode
    {
        ServiceProviderHasBeenModified = 1,
        UserNameIsDuplicated = 2,
        ServiceProviderHasBeenDeleted = 3,
        ServiceProviderIsPaused = 4,
        
        ServiceProviderDoesNotHaveRelatedAdmin = 5,
        ServiceProviderIsBeingSetUpForUsage = 6,
        
    }
}
