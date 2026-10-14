using System.Text.Json;

namespace Message_Agent.Common
{
    public class Settings
    {
        public string BrokerUrl { get; set; } = "https://127.0.0.1:7132";

        public string SubscriberUrl { get; set; } = "https://127.0.0.1:0";

        public static Settings Load()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "settings.json");

            if (!File.Exists(path))
            {
                return new Settings();
            }
            return JsonSerializer.Deserialize<Settings>(File.ReadAllText(path)) ?? new Settings();
        }
    }
}