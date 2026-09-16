using Message_Agent.Common;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace Receiver
{
    class PayloadHandler
    {
        private static Dictionary<string, List<string>> _messages =
            new Dictionary<string, List<string>>();

        private static object _locker = new object();

        public static void Handle(byte[] payloadBytes)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);
            var payload = JsonConvert.DeserializeObject<PayLoad>(payloadString);

            if (payload == null)
            {
                return;
            }

            lock (_locker)
            {
                if (!_messages.ContainsKey(payload.Topic))
                {
                    _messages[payload.Topic] = new List<string>();
                }

                _messages[payload.Topic].Add(payload.Message);
            }
        }

        public static Dictionary<string, List<string>> GetMessages()
        {
            lock (_locker)
            {
                return new Dictionary<string, List<string>>(_messages);
            }
        }
    }
}