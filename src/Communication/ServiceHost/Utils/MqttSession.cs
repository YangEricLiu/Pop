using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Pop.Communication.ServiceHost.Business;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SE.DSP.Pop.Communication.ServiceHost.Utils
{
    public static class MqttSession
    {
        private static string mqttServerAddress = ConfigHelper.Get("MqttServer");
        private static int mqttPort = Convert.ToInt32(ConfigHelper.Get("MqttPort"));

        private static MqttClient client = null;

        public static MqttClient Client 
        {
            get 
            {
                if (client == null)
                {
                    client = new MqttClient(mqttServerAddress, mqttPort, false, null);

                    string clientId = ConfigHelper.Get("MqttClientId");
                    string user = ConfigHelper.Get("MqttUserName");
                    string password = ConfigHelper.Get("MqttPassword");

                    client.Connect(clientId, user, password);
                }

                return client;
            }
        }

        public static void Disconnect()
        {
            Client.Disconnect();
        }

        public static void Subscribe(string topic, Action<object, MqttMsgPublishEventArgs> action)
        {
            var code = Client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            Client.MqttMsgPublishReceived += (o, e) =>
            {
                ////LogHelper.LogDebug("message received: " + e.Topic);
                action(o, e);
            };
        }

        public static void Publish(string topic, string message)
        {
            var code = Client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            LogHelper.LogDebug(string.Format("publish topic: {0}, message: {1}, result: {2}", topic, message, code));
        }
    }
}
