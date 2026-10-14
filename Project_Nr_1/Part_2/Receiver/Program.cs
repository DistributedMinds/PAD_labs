using Broker;
using Broker_gRPC.Services;
using Grpc.Net.Client;
using GrpcAgent;
using Message_Agent.Common;

var settings = Settings.Load();
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(settings.SubscriberUrl);

builder.Services.AddGrpc();
builder.Services.AddSingleton<IPersistentStore, PersistentStore>();

var app = builder.Build();

app.MapGrpcService<SubscriberService>();
app.MapGrpcService<NotifierService>();

app.Start();


bool running = true;
List<string> topics = new List<string>();

while (running)
{
    Console.WriteLine("1. Subscribe to topic");
    Console.WriteLine("2. Unsubscribe from topic");
    Console.WriteLine("3. View subscribed topics");
    Console.WriteLine("4. View received messages");
    Console.WriteLine("5. Exit");
    Console.WriteLine("===========================");
    Console.Write("Choose an option: ");

    string option = Console.ReadLine();

    switch (option)
    {
        case "1":
            Console.Write("Enter topic: ");
            string topic = Console.ReadLine().ToLower();

            if (topics.Contains(topic))
            {
                Console.WriteLine($"Already subscribed to topic: {topic}");
                break;
            }

            if (!string.IsNullOrWhiteSpace(topic))
            {
                Subscribe(app, topic);
            }
            break;

        case "2":
            Console.Write("Enter topic: ");
            string topicToUnsub = Console.ReadLine().ToLower();

            if (!topics.Contains(topicToUnsub))
            {
                Console.WriteLine($"Not subscribed to topic: {topicToUnsub}");
                break;
            }

            if (!string.IsNullOrWhiteSpace(topicToUnsub))
            {
                Unsubscribe(app, topicToUnsub);
            }
            break;

        case "3":
            Console.WriteLine();
            Console.WriteLine("----- SUBSCRIBED TOPICS -----");

            Console.WriteLine("------------------------------");
            break;

        case "4":
            Console.WriteLine();
            Console.WriteLine("===== RECEIVED MESSAGES =====");

            Console.WriteLine("=============================");
            break;

        case "5":
            running = false;
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

void Subscribe(WebApplication app, string topic)
{
    var channel = GrpcChannel.ForAddress(settings.BrokerUrl);
    var client = new Subscriber.SubscriberClient(channel);

    var address = app.Urls.First();

    var request = new SubscribeRequest
    {
        Topic = topic,
        Address = address
    };

    try
    {
        var reply = client.Subscribe(request);
        if (reply.Success)
        {
            Console.WriteLine($"Successfully subscribed to topic: {topic}");
            topics.Add(topic);
        }
        else
        {
            Console.WriteLine($"Failed to subscribe: {reply.Info}");
        }

    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while subscribing: {ex.Message}");
    }
}

void Unsubscribe(WebApplication app, string topic)
{
    var channel = GrpcChannel.ForAddress(settings.BrokerUrl);
    var client = new Subscriber.SubscriberClient(channel);

    var address = app.Urls.First();

    var request = new SubscribeRequest
    {
        Topic = topic,
        Address = address
    };

    try
    {
        var reply = client.Unsubscribe(request);
        if (reply.Success)
        {
            Console.WriteLine($"Successfully unsubscribed from topic: {topic}");
            topics.Remove(topic);
        }
        else
        {
            Console.WriteLine($"Failed to unsubscribe: {reply.Info}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while unsubscribing: {ex.Message}");
    }
}
