using GatewaySimulator.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace GatewaySimulator
{
    class Program
    {
        //test
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            //string command = args[0];
            //if (command == "reg")
            //{
            //    string boxName = args[1], boxMac = args[2];
            //    Register.RegesterCommand(boxName, boxMac);
            //}
            //HierarchyBusiness.PublishHierarchy();

            //Console.WriteLine("Press any key to continue...");
            //Console.Read();

            // create client instance
            //MqttClient client = new MqttClient("112.124.53.168", 22, false, null);

            //string clientId = "001";
            //client.Connect(clientId, "rem", "P@ssw0rd");
            //string strValue = Convert.ToString(123);
            //long i = 0;
            //while (true)
            //{
            //    // publish a message on "/home/temperature" topic with QoS 2
            //    client.Publish("/home/xbox", Encoding.UTF8.GetBytes("xbox" + i), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            //    Console.WriteLine("Send xbox" + i);

            //    Thread.Sleep(1000);
            //    i++;
            //} 
        }

    }
}
