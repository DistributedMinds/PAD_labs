using Broker;
using Grpc.Core;
using GrpcAgent;
using Message_Agent.Common.Models;

namespace Broker_gRPC.Services
{
    public class NotifierService: Notifier.NotifierBase
    {
        private readonly IPersistentStore _persistentStore;
        public NotifierService(IPersistentStore persistentStore)
        {
            _persistentStore = persistentStore;
        }
        public override Task<NotifyReply> Notify(NotifyRequest request, ServerCallContext context)
        {
            var payload = new PayLoad
            {
                Topic = request.Topic,
                Message = request.Message
            };
            _persistentStore.AddMessageWithDeliveries(payload);

            return Task.FromResult(new NotifyReply() { Success = true });
        }
    }
}
