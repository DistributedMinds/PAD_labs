using Message_Agent.Common;
using System.Collections.Generic;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== RECEIVER ==========");

            var receiverSocket = new ReceiverSocket();

            receiverSocket.Connect(Settings.BROKER_IP, Settings.BROKER_PORT);

            Console.Write("Sign up sau sign in? (1 = sign up, 2 = sign in): ");
            string choice = Console.ReadLine();

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            bool authenticated = receiverSocket.Authenticate(username, password, isSignUp: choice == "1");

            if (!authenticated)
            {
                Console.WriteLine("Could not authenticate. Exiting.");
                return;
            }

            Console.WriteLine("Authenticated successfully.");

            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("========== MENU ==========");
                Console.WriteLine("1. Subscribe to topic");
                Console.WriteLine("2. View subscribed topics");
                Console.WriteLine("3. View received messages");
                Console.WriteLine("4. Exit");
                Console.WriteLine("===========================");
                Console.Write("Choose an option: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Enter topic: ");

                        string topic = Console.ReadLine().ToLower();

                        if (!string.IsNullOrWhiteSpace(topic))
                        {
                            receiverSocket.Subscribe(topic);
                        }

                        break;

                    case "2":
                        Console.WriteLine();
                        Console.WriteLine("----- SUBSCRIBED TOPICS -----");

                        var topics = receiverSocket.GetTopics();

                        if (topics.Count == 0)
                        {
                            Console.WriteLine("No subscribed topics.");
                        }
                        else
                        {
                            foreach (string subscribedTopic in topics)
                            {
                                Console.WriteLine($"- {subscribedTopic}");
                            }
                        }

                        Console.WriteLine("------------------------------");
                        break;

                    case "3":
                        Console.WriteLine();
                        Console.WriteLine("===== RECEIVED MESSAGES =====");

                        var messages = PayloadHandler.GetMessages();

                        if (messages.Count == 0)
                        {
                            Console.WriteLine("No messages received.");
                        }
                        else
                        {
                            foreach (var topicMessages in messages)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"[{topicMessages.Key}]");

                                foreach (string message in topicMessages.Value)
                                {
                                    Console.WriteLine($"- {message}");
                                }
                            }
                        }

                        Console.WriteLine("=============================");
                        break;

                    case "4":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }

            Console.WriteLine("Receiver closed.");
        }
    }
}