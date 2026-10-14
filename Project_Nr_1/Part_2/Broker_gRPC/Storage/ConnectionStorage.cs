using Broker_gRPC.Models;

namespace Broker_gRPC.Storage
{
    public class ConnectionStorage : IConnectionStorage
    {
        private static List<Connection> _connections;
        private static object _locker;

        static ConnectionStorage()
        {
            _connections = new List<Connection>();
            _locker = new object();
        }

        public void Add(Connection connection)
        {
            lock (_locker)
            {
                _connections.RemoveAll(x => x.ClientId == connection.ClientId);
                _connections.Add(connection);
            }
        }

        public void Remove(string address)
        {
            lock (_locker)
            {
                _connections.RemoveAll(x => x.Address == address);
            }
        }

        public Connection? GetByClientId(string clientId)
        {
            lock (_locker)
            {
                return _connections.FirstOrDefault(x => x.ClientId == clientId);
            }
        }

        public List<Connection> GetConnectionsByTopic(string topic)
        {
            lock (_locker)
            {
                return _connections.Where(x => x.Topics.Contains(topic)).ToList();
            }
        }

        public List<Connection> GetAllConnected()
        {
            lock (_locker)
            {
                return _connections.ToList();
            }
        }
    }
}