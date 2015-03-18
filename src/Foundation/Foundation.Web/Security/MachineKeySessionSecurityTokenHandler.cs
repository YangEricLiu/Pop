
using System.Collections.Generic;
using System.IdentityModel;
using System.IdentityModel.Tokens;


namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// This class encrypts the session security token using the ASP.NET configured machine key.
    /// </summary>
    public class MachineKeySessionSecurityTokenHandler : SessionSecurityTokenHandler
    {
        static List<CookieTransform> _transforms;

        static MachineKeySessionSecurityTokenHandler()
        {
            _transforms = new List<CookieTransform>() 
                        { 
                            new DeflateCookieTransform(), 
                            new MachineKeyProtectionTransform()
                        };
        }

        public MachineKeySessionSecurityTokenHandler()
            : base(_transforms.AsReadOnly())
        {
        }
    }

}
