using GatewaySimulator.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewaySimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = args[0];
            if (command == "reg")
            {
                string boxName = args[1], boxMac = args[2];
                Register.RegesterCommand(boxName, boxMac);
            }

            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

    }
}
