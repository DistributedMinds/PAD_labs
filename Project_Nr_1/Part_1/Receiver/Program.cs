using Message_Agent.Common;
using System;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Receiver");

            string topic;
            Console.Write("Enter the topic: ");
            topic = Console.ReadLine().ToLower();

            var receiverSocket = new ReceiverSocket(topic);

            receiverSocket.Connect(Settings.BROKER_IP, Settings.BROKER_PORT);

            Console.WriteLine("Press any key to exit..");
            Console.ReadLine();

        }
    }
}