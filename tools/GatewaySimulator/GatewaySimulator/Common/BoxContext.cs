using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewaySimulator.Common
{
    public static class BoxContext
    {
        private static string boxIdDataFile = "boxid.data";

        private static string boxId;
        public static string BoxId
        {
            get 
            {
                if (string.IsNullOrEmpty(boxId))
                {
                    boxId = File.ReadAllText(boxIdDataFile, Encoding.UTF8);
                }

                return boxId;
            }
            set {
                File.WriteAllText(boxIdDataFile, value, Encoding.UTF8);
                boxId = value;
            }
        }
    }
}
