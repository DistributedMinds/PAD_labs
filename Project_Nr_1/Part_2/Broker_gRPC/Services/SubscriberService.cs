using Broker;
using Broker_gRPC.Models;
using Broker_gRPC.Storage;
using Grpc.Core;
using GrpcAgent;

namespace Broker_gRPC.Services
{
    public class SubscriberService : Subscriber.SubscriberBase
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
            string clientId = GetClientId(context);

            var connection = _connectionStorage.GetByClientId(clientId);
            if (connection == null)
            {
                connection = new Connection(request.Address) { ClientId = clientId };
                _connectionStorage.Add(connection);
            }

            if (!connection.Topics.Contains(request.Topic))
            {
                connection.Topics.Add(request.Topic);
            }

            _persistentStore.AddSubscription(clientId, request.Topic);

            Console.WriteLine($"Client {clientId} subscribed to topic: {request.Topic}");

            return Task.FromResult(new SubscribeReply { Success = true });
        }

        public override Task<SubscribeReply> Unsubscribe(SubscribeRequest request, ServerCallContext context)
        {
            string clientId = GetClientId(context);

            _connectionStorage.GetByClientId(clientId)?.Topics.Remove(request.Topic);

            _persistentStore.RemoveSubscription(clientId, request.Topic);

            Console.WriteLine($"Client {clientId} unsubscribed from topic: {request.Topic}");

            return Task.FromResult(new SubscribeReply { Success = true });
        }

        private static string GetClientId(ServerCallContext context)
        {
            if (context.UserState.TryGetValue(AuthInterceptor.UserKey, out var user) && user is string username)
                return username;

            throw new RpcException(new Status(StatusCode.Unauthenticated, "not authenticated"));
        }
    }
}