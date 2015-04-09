using GatewaySimulator.Business;
using GatewaySimulator.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace GatewaySimulator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.FormClosed += MainForm_FormClosed;

            Control.CheckForIllegalCrossThreadCalls = false;

            if (!string.IsNullOrEmpty(BoxContext.BoxId))
            {
                this.LabelRegisterStatus.Text = string.Format("Registered: {0}", BoxContext.BoxId);
                this.Subscribe();
            }
        }

        private void ButtonRegister_Click(object sender, EventArgs e)
        {
            this.LabelRegisterStatus.Text = "进行中";

            var name = this.TextBoxBoxName.Text.Trim();
            var mac = this.TextBoxBoxMac.Text.Trim();

            var response = RegisterBusiness.Register(name, mac);

            this.RegisterResult(response);
        }

        private void ButtonReplace_Click(object sender, EventArgs e)
        {
            this.LabelRegisterStatus.Text = "进行中";

            var name = this.TextBoxBoxName.Text.Trim();
            var mac = this.TextBoxBoxMac.Text.Trim();

            var response = RegisterBusiness.Replace(name, mac);

            this.RegisterResult(response);
        }

        private void RegisterResult(dynamic response)
        {
            var code = Convert.ToInt32(response.Error);

            var errorMap = new Dictionary<int, string> 
            { 
                {1,"Invalid name"},
                {2,"Invalid code"},
                {3,"Customer not exist"},
                {4,"Box already exist"},
                {5,"Box not exist"},
            };

            if (code == 0)
            {
                var result = response.Result;
                var boxId = result.BoxId;
                BoxContext.BoxId = boxId;

                this.Subscribe();

                this.LabelRegisterStatus.Text = string.Format("Succ, result is: {0}", result);
            }
            else if (code == -1)
            {
                this.LabelRegisterStatus.Text = "Server error!";
            }
            else
            {
                var message = errorMap[code];
                this.LabelRegisterStatus.Text = message;
            }
        }

        private void ButtonHierarchyUpload_Click(object sender, EventArgs e)
        {
            var text = this.TextBoxHierarchyText.Text.Trim();

            HierarchyBusiness.PublishHierarchy(text);

            this.LabelHierarchyStatus.Text = "data sent.";
        }

        private void Subscribe()
        {
            Action<MqttMsgPublishEventArgs> action = (e) =>
            {
                this.WriteLog(string.Format("topic:{0}", e.Topic));
                this.WriteLog(string.Format("message:{0}\n", Encoding.UTF8.GetString(e.Message)));
            };

            HierarchyBusiness.SubscribeAck((e) =>
            {
                this.LabelHierarchyStatus.Text = "ack received";
                action(e);
            });
            this.WriteLog("SubscribeAck");

            HierarchyBusiness.SubscribePush(action);
            this.WriteLog("SubscribePush");
        }

        private void WriteLog(string content)
        {
            this.OutputText.Text += string.Format("{0}\n", content);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        void MainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            MqttSession.Disconnect();
        }

    }
}
