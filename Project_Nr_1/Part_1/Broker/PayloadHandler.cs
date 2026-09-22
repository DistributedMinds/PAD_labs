using Message_Agent.Common;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Broker
{
    class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes, ConnectionInfo connectionInfo)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);

            if (payloadString.StartsWith("signup#") || payloadString.StartsWith("signin#"))
            {
                HandleAuth(payloadString, connectionInfo);
            }
            else if (payloadString.StartsWith("subscribe#"))
            {
                if (string.IsNullOrEmpty(connectionInfo.ClientId))
                {
                    SendResponse(connectionInfo, "error#not authenticated");
                    return;
                }

                string topic = payloadString.Substring("subscribe#".Length);

                if (!connectionInfo.Topics.Contains(topic))
                {
                    connectionInfo.Topics.Add(topic);
                }

                ConnectionsStorage.Add(connectionInfo);
                PersistentStore.AddSubscription(connectionInfo.ClientId, topic);

                Logger.Info($"Client {connectionInfo.ClientId} subscribed to topic: {topic}");
            }
            else if (payloadString.StartsWith("unsubscribe#"))
            {
                if (string.IsNullOrEmpty(connectionInfo.ClientId))
                {
                    SendResponse(connectionInfo, "error#not authenticated");
                    return;
                }

                string topic = payloadString.Substring("unsubscribe#".Length);

                connectionInfo.Topics.Remove(topic);
                PersistentStore.RemoveSubscription(connectionInfo.ClientId, topic);

                Logger.Info($"Client {connectionInfo.ClientId} unsubscribed from topic: {topic}");
            }
            else
            {
                try
                {

                    PayLoad payload;
                    string trimmedPayload = payloadString.TrimStart();
                    string format;

                    if (trimmedPayload.StartsWith("{"))
                    {
                        payload = JsonConvert.DeserializeObject<PayLoad>(trimmedPayload);
                        format = "JSON";
                    }
                    else if (trimmedPayload.StartsWith("<"))
                    {
                        var serializer = new XmlSerializer(typeof(PayLoad));
                        using var reader = new StringReader(trimmedPayload);
                        payload = (PayLoad)serializer.Deserialize(reader);
                        format = "XML";
                    }
                    else
                    {
                        Logger.Info($"Unknown payload format received from {connectionInfo.Address}");

                        return;
                    }

                    if (payload == null)
                    {
                        Logger.Info($"Invalid payload received from {connectionInfo.Address}");
                        return;
                    }

                    PersistentStore.AddMessageWithDeliveries(payload);
                    Logger.Info($"Message received from {connectionInfo.Address}. Topic: {payload.Topic}. Format: {format}");
                }
                catch (JsonException jsonEx)
                {
                    Logger.Info($"JSON deserialization error from {connectionInfo.Address}: {jsonEx.Message}");
                }
                catch (InvalidOperationException xmlEx)
                {
                    Logger.Info($"XML deserialization error from {connectionInfo.Address}: {xmlEx.Message}");
                }
                catch (Exception ex)
                {
                    Logger.Info($"Unexpected error while processing payload from {connectionInfo.Address}: {ex.Message}");
                }
                //PayLoad payload = JsonConvert.DeserializeObject<PayLoad>(payloadString);
                //PersistentStore.AddMessageWithDeliveries(payload);

                //Logger.Info($"Message received from {connectionInfo.Address}. Topic: {payload.Topic}");
            }
        }

        private static void HandleAuth(string payloadString, ConnectionInfo connectionInfo)
        {
            var parts = payloadString.Split('#');
            bool isSignUp = parts[0] == "signup";
            string username = parts[1];
            string password = parts[2];

            bool success = isSignUp
                ? PersistentStore.TryRegister(username, password)
                : PersistentStore.TryAuthenticate(username, password);

            if (success)
            {
                connectionInfo.ClientId = username;

                var previousTopics = PersistentStore.GetSubscribedTopics(username);

                foreach (var topic in previousTopics)
                {
                    if (!connectionInfo.Topics.Contains(topic))
                        connectionInfo.Topics.Add(topic);
                }

                ConnectionsStorage.Add(connectionInfo);

                string topicsJoined = string.Join(",", previousTopics);
                SendResponse(connectionInfo, $"authOK#{topicsJoined}");

                Logger.Info($"{connectionInfo.Address} authenticated as '{username}' ({(isSignUp ? "signup" : "signin")}). Restored topics: {topicsJoined}");
            }
            else
            {
                SendResponse(connectionInfo, isSignUp ? "authFAIL#username taken" : "authFAIL#invalid credentials");
            }
        }

        private static void SendResponse(ConnectionInfo connectionInfo, string message)
        {
            connectionInfo.Socket.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}