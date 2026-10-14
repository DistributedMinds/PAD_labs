using Broker;
using Broker_gRPC.Models;
using Broker_gRPC.Storage;
using Grpc.Core;
using GrpcAgent;

namespace Broker_gRPC.Services
{
    public class SubscriberService: Subscriber.SubscriberBase
    {
        private readonly IPersistentStore _persistentStore;
        private readonly IConnectionStorage _connectionStorage;
        public SubscriberService(IPersistentStore persistentStore, IConnectionStorage connectionStorage)
        {
            _persistentStore = persistentStore;
            _connectionStorage = connectionStorage;
        }
        public override Task<SubscribeReply> Subscribe(SubscribeRequest request, ServerCallContext context)
        {

            var connection = new Connection(request.Address);
            _connectionStorage.Add(connection);

            _persistentStore.AddSubscription(request.Address, request.Topic);

            Console.WriteLine($"{request.Address} subscribed to topic: {request.Topic}");

            return Task.FromResult(new SubscribeReply { Success = true });
        }

        public override Task<SubscribeReply> Unsubscribe(SubscribeRequest request, ServerCallContext context)
        {
            _connectionStorage.Remove(request.Address);

            _persistentStore.RemoveSubscription(request.Address, request.Topic);
            Console.WriteLine($"{request.Address} unsubscribed from topic: {request.Topic}");
            return Task.FromResult(new SubscribeReply { Success = true });
        }
    }
}
