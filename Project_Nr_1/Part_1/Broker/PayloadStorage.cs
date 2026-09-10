using Message_Agent.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    class PayloadStorage
    {
        private static ConcurrentQueue<PayLoad> _payloadQueue;

        static PayloadStorage()
        {
            _payloadQueue = new ConcurrentQueue<PayLoad>();
        }

        public static void Add(PayLoad payload)
        {
            _payloadQueue.Enqueue(payload);
        }

        public static PayLoad GetNext()
        {
            PayLoad payload = null;

            _payloadQueue.TryDequeue(out payload);
            return payload;

        }

        public static bool IsEmpty()
        {
            return _payloadQueue.IsEmpty;

        }
             
    }
}
