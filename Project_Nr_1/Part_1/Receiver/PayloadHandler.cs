using Message_Agent.Common;
using Newtonsoft.Json;
using System.Text;

namespace Receiver
{
    class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);
            var payload = JsonConvert.DeserializeObject<PayLoad>(payloadString);

            Console.WriteLine(payload.Message);
        }
    }
}
