
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    while (!PayloadStorage.IsEmpty())
                    {
                        var payload = PayloadStorage.GetNext();

                        if (payload != null)
                        {
                            var connections =
                                ConnectionsStorage.GetConnectionsByTopic(payload.Topic);

                            Logger.Info(
                                $"Routing message with topic '{payload.Topic}' to {connections.Count} receiver(s)."
                            );

                            foreach (var connection in connections)
                            {
                                var payloadString =
                                    JsonConvert.SerializeObject(payload);

                                byte[] data =
                                    Encoding.UTF8.GetBytes(payloadString);

                                connection.Socket.Send(data);

                                Logger.Info(
                                    $"Message delivered to {connection.Address}. Topic: {payload.Topic}"
                                );
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
