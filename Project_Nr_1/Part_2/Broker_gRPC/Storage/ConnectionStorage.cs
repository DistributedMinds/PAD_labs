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
                if (!_connections.Any(x => x.Address == connection.Address))
                {
                    _connections.Add(connection);
                }
            }
        }

        public void Remove(string address)
        {
            lock (_locker)
            {
                _connections.RemoveAll(x => x.Address == address);
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
