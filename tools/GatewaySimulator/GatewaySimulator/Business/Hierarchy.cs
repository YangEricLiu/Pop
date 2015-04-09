using GatewaySimulator.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace GatewaySimulator
{
    public class HierarchyBusiness
    {
        private string Version = ConfigurationManager.AppSettings["MessageVersion"];

        private static string MqttServerAddress = ConfigurationManager.AppSettings["MqttServer"];
        private static int MqttPort = Convert.ToInt32(ConfigurationManager.AppSettings["MqttPort"]);
        
        public static void PublishHierarchy(string hierarchyText)
        {
            string topic = "/V1.0/hsend/" + BoxContext.BoxId;

            MqttSession.Publish(topic, hierarchyText);
        }

        public static void SubscribeAck(Action<MqttMsgPublishEventArgs> messageArrive)
        {
            string topic = "/V1.0/hcack/" + BoxContext.BoxId;

            Action<object, MqttMsgPublishEventArgs> action = (o, e) =>
            {
                if (e.Topic == topic.Replace(".","/"))
                {
                    messageArrive(e);
                }
            };

            MqttSession.Subscribe(topic, action); 
        }

        public static void SubscribePush(Action<MqttMsgPublishEventArgs> messageArrive)
        {
            string topic = "/V1.0/hpush/" + BoxContext.BoxId;

            Action<object, MqttMsgPublishEventArgs> action = (o, e) =>
            {
                if (e.Topic == topic.Replace(".", "/"))
                {
                    messageArrive(e);

                    //box's business

                    //after processed business, send ack
                    PublishAck();
                }
            };

            MqttSession.Subscribe(topic, action); 
        }


        public static void PublishAck()
        {
            string topic = "/V1.0/hack/" + BoxContext.BoxId;
            MqttSession.Publish(topic, "ok");
        }

    }
}
