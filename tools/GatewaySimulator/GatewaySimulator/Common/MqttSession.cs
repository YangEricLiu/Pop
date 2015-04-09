using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace GatewaySimulator.Common
{
    public static class MqttSession
    {
        private static string mqttServerAddress = ConfigurationManager.AppSettings["MqttServer"];
        private static int mqttPort = Convert.ToInt32(ConfigurationManager.AppSettings["MqttPort"]);

        private static MqttClient client = null;

        public static MqttClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new MqttClient(mqttServerAddress, mqttPort, false, null);

                    string clientId = BoxContext.BoxId;
                    string user = ConfigurationManager.AppSettings["MqttUserName"];
                    string password = ConfigurationManager.AppSettings["MqttPassword"];

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
                action(o, e);
            };
        }

        public static void Publish(string topic, string message)
        {
            var code = Client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
        }
    }
}
