using Grpc.Core;
using GrpcAgent;

namespace Receiver.Services;

public class MessageReceiverService : Notifier.NotifierBase
{
    private readonly ReceivedMessagesStore _store;

    public MessageReceiverService(ReceivedMessagesStore store)
    {
        _store = store;
    }

    public override Task<NotifyReply> Notify(NotifyRequest request, ServerCallContext context)
    {
        _store.Add(request.Topic, request.Message);

        return Task.FromResult(new NotifyReply { Success = true });
    }
}