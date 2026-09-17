using Message_Agent.Common;
using Microsoft.Data.Sqlite;

namespace Broker;

public class PersistentStore
{
    private static readonly SqliteConnection _connection;
        private static readonly object _lock = new object();
        
        private static string GetProjectRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && dir.GetFiles("*.csproj").Length == 0)
                dir = dir.Parent;

            return dir?.FullName ?? AppContext.BaseDirectory;
        }

        static PersistentStore()
        {
            
            
            try
            {
                string dataDir = Path.Combine(GetProjectRoot(), "Data");
                Directory.CreateDirectory(dataDir);

                string dbPath = Path.Combine(dataDir, "broker_messages.db");

                _connection = new SqliteConnection($"Data Source={dbPath}");
                _connection.Open();

                using (var pragma = _connection.CreateCommand())
                {
                    pragma.CommandText = "PRAGMA journal_mode=DELETE;";
                    pragma.ExecuteNonQuery();
                }

                using (var createTable = _connection.CreateCommand())
                {
                    createTable.CommandText =
                        @"CREATE TABLE IF NOT EXISTS messages (
                    Id TEXT PRIMARY KEY,
                    Topic TEXT NOT NULL,
                    Message TEXT NOT NULL,
                    Status INTEGER NOT NULL,
                    CreatedAtUtc TEXT NOT NULL
                );";
                    createTable.ExecuteNonQuery();
                }

                Console.WriteLine("PersistentStore initialized OK.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"PersistentStore init FAILED: {e}");
            }
        }

        public static void Add(PayLoad payload)
        {
            lock (_lock)
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText =
                    @"INSERT INTO messages (Id, Topic, Message, Status, CreatedAtUtc)
                      VALUES ($id, $topic, $message, $status, $createdAt);";

                cmd.Parameters.AddWithValue("$id", payload.Id.ToString());
                cmd.Parameters.AddWithValue("$topic", payload.Topic);
                cmd.Parameters.AddWithValue("$message", payload.Message);
                cmd.Parameters.AddWithValue("$status", (int)PayloadStatus.Pending);
                cmd.Parameters.AddWithValue("$createdAt", payload.CreatedAtUtc.ToString("o"));

                cmd.ExecuteNonQuery();
            }
        }

        public static void MarkDelivered(Guid id)
        {
            lock (_lock)
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = "UPDATE messages SET Status = $status WHERE Id = $id;";
                cmd.Parameters.AddWithValue("$status", (int)PayloadStatus.Delivered);
                cmd.Parameters.AddWithValue("$id", id.ToString());

                cmd.ExecuteNonQuery();
            }
        }

        public static List<PayLoad> LoadPending()
        {
            var result = new List<PayLoad>();

            lock (_lock)
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText =
                    "SELECT Id, Topic, Message, Status, CreatedAtUtc FROM messages WHERE Status = $status;";
                cmd.Parameters.AddWithValue("$status", (int)PayloadStatus.Pending);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new PayLoad
                    {
                        Id = Guid.Parse(reader.GetString(0)),
                        Topic = reader.GetString(1),
                        Message = reader.GetString(2),
                        Status = (PayloadStatus)reader.GetInt32(3),
                        CreatedAtUtc = DateTime.Parse(reader.GetString(4))
                    });
                }
            }

            return result;
        }
        
        public static void PrintRowCount()
        {
            lock (_lock)
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM messages;";
                var count = cmd.ExecuteScalar();
                Console.WriteLine($"Rows in DB: {count}");
            }
        }
}