namespace Receiver.Services;

public class ReceivedMessagesStore
{
    private readonly Dictionary<string, List<string>> _messages = new();
    private readonly object _locker = new();

    public void Add(string topic, string message)
    {
        lock (_locker)
        {
            if (!_messages.ContainsKey(topic))
            {
                _messages[topic] = new List<string>();
            }

            _messages[topic].Add(message);
        }
    }

    public Dictionary<string, List<string>> GetAll()
    {
        lock (_locker)
        {
            return _messages.ToDictionary(x => x.Key, x => x.Value.ToList());
        }
    }
}