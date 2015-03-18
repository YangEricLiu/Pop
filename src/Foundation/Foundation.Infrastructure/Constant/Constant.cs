using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Constant
{
    public static class Constant
    {
        public const int MAXORGANIZATIONNESTING = 5;
        public const int MAXAREANESTING = 5;

        public const long ALLCUSTOMER = 0;
        public const long INITPLATFORMADMINISTRATOR = 100001;

        public const int DEMOUSERTOKENEXPIRATIONRELATIVEDAYS = 1;
        public const int PASSWORDTOKENEXPIRATIONRELATIVEDAYS = 1;
        public const int INITPASSWORDTOKENEXPIRATIONABSOLUTEYEAR = 3000;

        public const decimal MINTBVALUE = -999999999.999999m;
        public const decimal MAXTBVALUE = 999999999.999999m;

        public const string DEMOUSERPASSWORD= "demo@DEMO2013rem";
    }
}