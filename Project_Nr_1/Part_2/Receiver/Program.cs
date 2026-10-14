using Broker;
using Broker_gRPC.Services;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcAgent;
using Message_Agent.Common;

var settings = Settings.Load();
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(o =>
    o.ListenAnyIP(settings.ReceiverPort, l => l.UseHttps()));

builder.Services.AddGrpc();
builder.Services.AddSingleton<IPersistentStore, PersistentStore>();

var app = builder.Build();

app.MapGrpcService<NotifierService>();

app.Start();

var channel = GrpcChannel.ForAddress(settings.BrokerUrl);
var authClient = new Authenticator.AuthenticatorClient(channel);
var subscriberClient = new Subscriber.SubscriberClient(channel);

string receiverAddress = app.Urls.First();
string? token = null;
List<string> topics = new List<string>();

Console.WriteLine("========== RECEIVER ==========");

// ---------- Autentificare (ca în Part 1) ----------
bool authenticated = false;

while (!authenticated)
{
    Console.WriteLine();
    Console.Write("Sign up sau sign in? (1 = sign up, 2 = sign in): ");
    string? choice = Console.ReadLine();

    if (choice != "1" && choice != "2")
    {
        Console.WriteLine("Opțiune invalidă. Alege 1 sau 2.");
        continue;
    }

    Console.Write("Username: ");
    string username = Console.ReadLine() ?? "";

    Console.Write("Password: ");
    string password = Console.ReadLine() ?? "";

    var result = Authenticate(username, password, isSignUp: choice == "1");

    if (result.Success)
    {
        authenticated = true;
        Console.WriteLine(result.Message);
        
    }
    else
    {
        Console.WriteLine($"Autentificare eșuată: {result.Message}. Încearcă din nou.");
    }
}

// ---------- Meniu ----------
bool running = true;

while (running)
{
    Console.WriteLine("1. Subscribe to topic");
    Console.WriteLine("2. Unsubscribe from topic");
    Console.WriteLine("3. View subscribed topics");
    Console.WriteLine("4. View received messages");
    Console.WriteLine("5. Exit");
    Console.WriteLine("===========================");
    Console.Write("Choose an option: ");

    string? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            Console.Write("Enter topic: ");
            string topic = (Console.ReadLine() ?? "").ToLower();

            if (topics.Contains(topic))
            {
                Console.WriteLine($"Already subscribed to topic: {topic}");
                break;
            }

            if (!string.IsNullOrWhiteSpace(topic))
            {
                Subscribe(topic);
            }
            break;

        case "2":
            Console.Write("Enter topic: ");
            string topicToUnsub = (Console.ReadLine() ?? "").ToLower();

            if (!topics.Contains(topicToUnsub))
            {
                Console.WriteLine($"Not subscribed to topic: {topicToUnsub}");
                break;
            }

            if (!string.IsNullOrWhiteSpace(topicToUnsub))
            {
                Unsubscribe(topicToUnsub);
            }
            break;

        case "3":
            Console.WriteLine();
            Console.WriteLine("----- SUBSCRIBED TOPICS -----");

            if (topics.Count == 0)
            {
                Console.WriteLine("No subscribed topics.");
            }
            else
            {
                foreach (var t in topics)
                {
                    Console.WriteLine($"- {t}");
                }
            }

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

// ---------- Funcții locale ----------

(bool Success, string Message) Authenticate(string username, string password, bool isSignUp)
{
    var request = new AuthRequest
    {
        Username = username,
        Password = password,
        Address = receiverAddress
    };

    try
    {
        var reply = isSignUp ? authClient.SignUp(request) : authClient.SignIn(request);

        if (!reply.Success)
        {
            return (false, reply.Info);
        }

        token = reply.Token;

        foreach (var t in reply.Topics)
        {
            if (!topics.Contains(t))
                topics.Add(t);
        }

        return (true, "Authenticated successfully.");
    }
    catch (RpcException ex)
    {
        return (false, ex.Status.Detail);
    }
    catch (Exception ex)
    {
        return (false, ex.Message);
    }
}

Metadata AuthHeaders()
{
    return new Metadata { { "authorization", $"Bearer {token}" } };
}

void Subscribe(string topic)
{
    var request = new SubscribeRequest
    {
        Topic = topic,
        Address = receiverAddress
    };

    try
    {
        var reply = subscriberClient.Subscribe(request, AuthHeaders());
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

void Unsubscribe(string topic)
{
    var request = new SubscribeRequest
    {
        Topic = topic,
        Address = receiverAddress
    };

    try
    {
        var reply = subscriberClient.Unsubscribe(request, AuthHeaders());
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