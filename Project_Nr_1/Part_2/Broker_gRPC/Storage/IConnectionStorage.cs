using Broker_gRPC.Models;

namespace Broker_gRPC.Storage
{
    public interface IConnectionStorage
    {
        void Add(Connection connection);
        void Remove(string address);
        List<Connection> GetAllConnected();
        List<Connection> GetConnectionsByTopic(string topic);

    }
}
