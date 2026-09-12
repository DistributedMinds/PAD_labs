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
                connectionInfo.Topic = payloadString.Split("subscribe#").LastOrDefault();
               ConnectionsStorage.Add(connectionInfo);
            }
            else
            {
                PayLoad payload = JsonConvert.DeserializeObject<PayLoad>(payloadString);
                //adaugam in storage

                PayloadStorage.Add(payload);
               // Console.Write(payloadString);
            }


                
        }
    }
}
