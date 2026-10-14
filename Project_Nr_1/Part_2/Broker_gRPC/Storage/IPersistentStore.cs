using Message_Agent.Common.Models;

namespace Broker;

public interface IPersistentStore
{
    bool TryRegister(string username, string password);
    bool TryAuthenticate(string username, string password);

    List<string> GetSubscribedTopics(string clientId);
    void AddSubscription(string clientId, string topic);
    void RemoveSubscription(string clientId, string topic);

    void AddMessageWithDeliveries(PayLoad payload);
    List<PayLoad> GetPendingDeliveries(string clientId);
    void RemovePendingDeliveries(string clientId, string topic);
    void MarkDelivered(Guid messageId, string clientId);
}