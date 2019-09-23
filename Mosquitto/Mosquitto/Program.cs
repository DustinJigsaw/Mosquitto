using System;
using System.Net.Mqtt;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace daenet.iot
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Console.WriteLine("Hello, World!");

            var mqttClient = MqttClient.CreateAsync("localhost").Result;

            var sess = mqttClient.ConnectAsync().Result;

            string rcvTopic = "et12/lab22";
            string sendTopic = "et12/lab22";

            mqttClient.SubscribeAsync(rcvTopic, MqttQualityOfService.ExactlyOnce);

            Task.Run(() =>
            {
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine("Enter the text to send.");

                    Console.ForegroundColor = ConsoleColor.Cyan;

                    var line = System.Console.ReadLine();

                    var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(line));

                    mqttClient.PublishAsync(new MqttApplicationMessage(sendTopic, data), MqttQualityOfService.ExactlyOnce).Wait();
                }
            });

            mqttClient.MessageStream.Subscribe(msg =>
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine(Encoding.UTF8.GetString(msg.Payload));

                Console.ResetColor();
            });
        }
    }
}