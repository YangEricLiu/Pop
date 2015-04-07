using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Pop.Communication.ServiceHost;

namespace ServiceHost
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] 
            { 
                new MainService() 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
