using Message_Agent.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes, ConnectionInfo connectionInfo)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);

            if (payloadString.StartsWith("subscribe#"))
            {
                string topic = payloadString.Substring("subscribe#".Length);

                if (!connectionInfo.Topics.Contains(topic))
                {
                    connectionInfo.Topics.Add(topic);
                }

                ConnectionsStorage.Add(connectionInfo);

                Logger.Info(
                    $"Client {connectionInfo.Address} subscribed to topic: {topic}"
                );
            }
            else
            {
                PayLoad payload = JsonConvert.DeserializeObject<PayLoad>(payloadString);
                //adaugam in storage
                PersistentStore.Add(payload);
                PayloadStorage.Add(payload);
                // Console.Write(payloadString);

                Logger.Info(
                    $"Message received from {connectionInfo.Address}. Topic: {payload.Topic}"
);
            }


                
        }
    }
}
