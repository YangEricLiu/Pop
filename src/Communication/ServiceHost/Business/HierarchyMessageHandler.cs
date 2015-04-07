using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Pop.Communication.ServiceHost.Utils;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SE.DSP.Pop.Communication.ServiceHost.Business
{
    public static class HierarchyMessageHandler
    {
        //// 1. Box upload hierarchy cloud
        ////    (1). box send hierarchy data                              (V1.0/hsend/{boxid})
        ////    (2). cloud handle message by sub data send topic          (V1.0/hsend/+)
        ////    (3). cloud send ack to box after finish processing data   (V1.0/hack/{boxid})
        
        //// 2. Cloud push hierarchy to box
        ////    (1). cloud send hierarchy data                            (V1.0/hpush/{boxid})
        ////    (2). box handle message by sub data push topic            (V1.0/hpush/{boxid})
        ////    (3). box send ack to cloud after finish processing data   (V1.0/hack/{boxid})
        ////    (4). cloud get ack by sub ack topic                       (V1.0/hack/+)

        //// 3. Cloud inquire hierarchy from box
        ////    (1). cloud send inquire command                           (V1.0/hget/{boxid})
        ////    (2). box receiving inquire command by sub get topic       (V1.0/hget/{boxid})
        ////    (3). box send data using process defined in 1

        public const string SubscribeDataTopic = "/V1/0/hsend/+";
        public const string SubscribeAckTopic = "/V1/0/hack/+";

        private const string PublishDataTopic = "/V1.0/hpush/{0}";
        private const string PublishAckTopic = "V1.0/hack/{0}";
        private const string PublishInquireTopic = "/V1.0/hget/{0}";

        public static void SubDataTopic()
        {
            Action<object, MqttMsgPublishEventArgs> action = (o, e) =>
            {
                ////LogHelper.LogDebug("hierarchy data: " + e.Topic);

                if (e.Topic.StartsWith(SubscribeDataTopic.Substring(0, SubscribeDataTopic.Length - 1)))
                {
                    LogHelper.LogDebug("hierarchy data message: " + Encoding.UTF8.GetString(e.Message));
                }
            };

            MqttSession.Subscribe(SubscribeDataTopic, action);
        }

        public static void SubAckTopic()
        {
            Action<object, MqttMsgPublishEventArgs> action = (o, e) =>
            {
                ////LogHelper.LogDebug("hierarchy ack: " + e.Topic);

                if (e.Topic.StartsWith(SubscribeAckTopic.Substring(0, SubscribeAckTopic.Length - 1)))
                {
                    var id = e.Topic.Split('/').LastOrDefault();

                    LogHelper.LogDebug("ack:" + (string.IsNullOrEmpty(id) ? "null" : id));
                }
            };

            MqttSession.Subscribe(SubscribeAckTopic, action);
        }

        public static void Ack(string boxId)
        {
            string topic = "V1.0/hack/{0}";
            topic = string.Format(topic, boxId);                
        }

        public static void Inquire(string boxId)
        {
            string topic = "V1.0/hget/{0}";
            topic = string.Format(topic, boxId);
        }

        public static void Push(string boxId)
        { 
            string topic = "V1.0/hpush/{0}";
            topic = string.Format(topic, boxId);
        }
    }
}
