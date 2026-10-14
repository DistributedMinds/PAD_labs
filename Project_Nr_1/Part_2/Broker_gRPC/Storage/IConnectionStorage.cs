using Broker_gRPC.Models;

namespace Broker_gRPC.Storage
{
    public interface IConnectionStorage
    {
        void Add(Connection connection);
        void Remove(string address);
        Connection? GetByClientId(string clientId);
        List<Connection> GetAllConnected();
        List<Connection> GetConnectionsByTopic(string topic);

    }
}
