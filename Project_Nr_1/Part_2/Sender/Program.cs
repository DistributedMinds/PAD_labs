using Grpc.Core;
using Grpc.Net.Client;
using GrpcAgent;
using Message_Agent.Common;

var settings = Settings.Load();

using var channel = GrpcChannel.ForAddress(settings.BrokerUrl);
var client = new Publisher.PublisherClient(channel);

Console.WriteLine("Sender");

while (true)
{
    Console.Write("Enter topic: ");
    string topic = Console.ReadLine().ToLower();

    Console.Write("Enter message: ");
    string message = Console.ReadLine().ToLower();
    var request = new PublishRequest
    {
        Topic = topic,
        Message = message
    };

    try
    {
       var reply = await client.PublishMessageAsync(request);
       Console.WriteLine($"info{reply.Info}, deliveres with success = {reply.Success}");
    }
    catch (RpcException ex)
    {
        Console.WriteLine($"gRPC error: {ex.Status.Detail}");
    }
}