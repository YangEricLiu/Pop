using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IO;
using SE.DSP.Pop.Communication.ServiceHost.API;
using SE.DSP.Pop.Communication.ServiceHost.Services;

namespace SE.DSP.Pop.Communication.ServiceHost
{
    public partial class MainService : ServiceBase
    {
        private System.ServiceModel.ServiceHost serviceHost = null;
        private StreamWriter writer = new StreamWriter(@"e:\temp\service-log.txt", true);

        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                writer.WriteLine("service starting");
                writer.Flush();

                if (serviceHost != null)
                {
                    serviceHost.Close();
                }

                serviceHost = new System.ServiceModel.ServiceHost(typeof(CommunicationService));
                serviceHost.Open();

                writer.WriteLine("service started!");
                writer.Flush();
            }
            catch (Exception ex)
            {
                writer.WriteLine(ex.Message);
                writer.WriteLine(ex.StackTrace);
            }
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }

            writer.Flush();
            writer.Close();
        }
    }
}
