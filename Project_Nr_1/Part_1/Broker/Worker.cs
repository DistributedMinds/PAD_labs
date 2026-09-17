using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;

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
                    var pendingPayloads = PersistentStore.LoadPending();

                    foreach (var payload in pendingPayloads)
                    {
                        var connections = ConnectionsStorage.GetConnectionsByTopic(payload.Topic);

                        if (connections.Count == 0)
                        {
                            continue; // rămâne Pending, se reverifică la tick-ul următor
                        }

                        Logger.Info(
                            $"Routing message with topic '{payload.Topic}' to {connections.Count} receiver(s)."
                        );

                        bool allSucceeded = true;

                        foreach (var connection in connections)
                        {
                            try
                            {
                                var payloadString = JsonConvert.SerializeObject(payload);
                                byte[] data = Encoding.UTF8.GetBytes(payloadString);
                                connection.Socket.Send(data);

                                Logger.Info($"Message delivered to {connection.Address}. Topic: {payload.Topic}");
                            }
                            catch (Exception e)
                            {
                                allSucceeded = false;
                                Logger.Error($"Delivery failed to {connection.Address}: {e.Message}");
                            }
                        }

                        if (allSucceeded)
                        {
                            PersistentStore.MarkDelivered(payload.Id);
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