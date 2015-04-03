using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Communication.ServiceHost.Business
{
    public class HierarchyMessageHandler
    {
        // 1. Box upload hierarchy cloud
        //    (1). box send hierarchy data                              (V1.0/hsend/{boxid})
        //    (2). cloud handle message by sub data send topic          (V1.0/hsend/+)
        //    (3). cloud send ack to box after finish processing data   (V1.0/hack/{boxid})
        
        // 2. Cloud push hierarchy to box
        //    (1). cloud send hierarchy data                            (V1.0/hpush/{boxid})
        //    (2). box handle message by sub data push topic            (V1.0/hpush/{boxid})
        //    (3). box send ack to cloud after finish processing data   (V1.0/hack/{boxid})
        //    (4). cloud get ack by sub ack topic                       (V1.0/hack/+)

        // 3. Cloud inquire hierarchy from box
        //    (1). cloud send inquire command                           (V1.0/hget/{boxid})
        //    (2). box receiving inquire command by sub get topic       (V1.0/hget/{boxid})
        //    (3). box send data using process defined in 1

        public static void SubDataTopic()
        {
            string topic = "V1.0/hsend/+";
            
        }

        public static void SubAckTopic()
        {
            string topic = "V1.0/hack/+";
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
