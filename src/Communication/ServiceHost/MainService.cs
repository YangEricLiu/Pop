using System;
using System.ServiceProcess;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Pop.Communication.ServiceHost.Business;
using SE.DSP.Pop.Communication.ServiceHost.Services;
using SE.DSP.Pop.Communication.ServiceHost.Utils;

namespace SE.DSP.Pop.Communication.ServiceHost
{
    public partial class MainService : ServiceBase
    {
        private System.ServiceModel.ServiceHost serviceHost = null;

        public MainService()
        {
            this.InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (this.serviceHost != null)
                {
                    this.serviceHost.Close();
                }

                this.serviceHost = new System.ServiceModel.ServiceHost(typeof(CommunicationService));
                this.serviceHost.Open();

                this.Subscribe();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (this.serviceHost != null)
                {
                    this.serviceHost.Close();
                    this.serviceHost = null;

                    MqttSession.Disconnect();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
        }

        private void Subscribe()
        {
            HierarchyMessageHandler.SubDataTopic();
            HierarchyMessageHandler.SubAckTopic();
        }
    }
}
