using Newtonsoft.Json;
using System.Text;

namespace Broker
{
    class Worker
    {
        private const int TIME_TO_SLEEP = 500;

        public void DoSendMessageWork()
        {
            Logger.Info("Worker started.");

            while (true)
            {
                try
                {
                    var connections = ConnectionsStorage.GetAllConnected();

                    foreach (var connection in connections)
                    {
                        if (string.IsNullOrEmpty(connection.ClientId))
                        {
                            continue;
                        }

                        var pending = PersistentStore.GetPendingDeliveries(connection.ClientId);

                        foreach (var payload in pending)
                        {
                            try
                            {
                                var payloadString = JsonConvert.SerializeObject(payload);
                                byte[] data = Encoding.UTF8.GetBytes(payloadString);
                                connection.Socket.Send(data);

                                PersistentStore.MarkDelivered(payload.Id, connection.ClientId);

                                Logger.Info($"Delivered message {payload.Id} to {connection.ClientId}");
                            }
                            catch (Exception e)
                            {
                                Logger.Error($"Delivery failed to {connection.Address}: {e.Message}");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error($"Worker error: {e.Message}");
                }

                Thread.Sleep(TIME_TO_SLEEP);
            }
        }
    }
}