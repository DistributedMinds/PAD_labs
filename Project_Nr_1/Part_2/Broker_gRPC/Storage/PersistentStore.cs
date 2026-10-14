using System.Security.Cryptography;
using System.Text;
using Message_Agent.Common.Models;
using Microsoft.Data.Sqlite;

namespace Broker;

public class PersistentStore : IPersistentStore
{
    private readonly SqliteConnection _connection;
    private readonly object _lock = new object();

    public PersistentStore()
    {
        string dbPath = DatabaseInitializer.GetDatabasePath();

        _connection = new SqliteConnection($"Data Source={dbPath}");
        _connection.Open();

        DatabaseInitializer.CreateTables(_connection);
    }

    // --- Autentificare ---

    public bool TryRegister(string username, string password)
    {
        lock (_lock)
        {
            using (var check = _connection.CreateCommand())
            {
                check.CommandText = "SELECT COUNT(*) FROM users WHERE Username = $u;";
                check.Parameters.AddWithValue("$u", username);
                long count = (long)check.ExecuteScalar();
                if (count > 0)
                    return false;
            }

            using var insert = _connection.CreateCommand();
            insert.CommandText = "INSERT INTO users (Username, PasswordHash) VALUES ($u, $p);";
            insert.Parameters.AddWithValue("$u", username);
            insert.Parameters.AddWithValue("$p", HashPassword(password));
            insert.ExecuteNonQuery();
            return true;
        }
    }

    public bool TryAuthenticate(string username, string password)
    {
        lock (_lock)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT PasswordHash FROM users WHERE Username = $u;";
            cmd.Parameters.AddWithValue("$u", username);
            var storedHash = cmd.ExecuteScalar() as string;

            return storedHash != null && storedHash == HashPassword(password);
        }
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    // --- Subscripții ---
    public List<string> GetSubscribedTopics(string clientId)
    {
        var result = new List<string>();

        lock (_lock)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT Topic FROM subscriptions WHERE ClientId = $clientId;";
            cmd.Parameters.AddWithValue("$clientId", clientId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader.GetString(0));
            }
        }

        return result;
    }

    public void AddSubscription(string clientId, string topic)
    {
        lock (_lock)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText =
                "INSERT OR IGNORE INTO subscriptions (ClientId, Topic) VALUES ($clientId, $topic);";
            cmd.Parameters.AddWithValue("$clientId", clientId);
            cmd.Parameters.AddWithValue("$topic", topic);
            cmd.ExecuteNonQuery();
        }
    }
    public void RemoveSubscription(string clientId, string topic)
    {
        lock (_lock)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText =
                "DELETE FROM subscriptions WHERE ClientId = $clientId AND Topic = $topic;";
            cmd.Parameters.AddWithValue("$clientId", clientId);
            cmd.Parameters.AddWithValue("$topic", topic);
            cmd.ExecuteNonQuery();
        }
    }

    // --- Mesaje + livrări ---

    public void AddMessageWithDeliveries(PayLoad payload)
    {
        lock (_lock)
        {
            using var transaction = _connection.BeginTransaction();

            using (var insertMsg = _connection.CreateCommand())
            {
                insertMsg.Transaction = transaction;
                insertMsg.CommandText =
                    @"INSERT INTO messages (Id, Topic, Message, CreatedAtUtc)
                      VALUES ($id, $topic, $message, $createdAt);";
                insertMsg.Parameters.AddWithValue("$id", payload.Id.ToString());
                insertMsg.Parameters.AddWithValue("$topic", payload.Topic);
                insertMsg.Parameters.AddWithValue("$message", payload.Message);
                insertMsg.Parameters.AddWithValue("$createdAt", payload.CreatedAtUtc.ToString("o"));
                insertMsg.ExecuteNonQuery();
            }

            using (var insertDeliveries = _connection.CreateCommand())
            {
                insertDeliveries.Transaction = transaction;
                insertDeliveries.CommandText =
                    @"INSERT INTO deliveries (MessageId, ClientId, Status)
                      SELECT $messageId, ClientId, 0 FROM subscriptions WHERE Topic = $topic;";
                insertDeliveries.Parameters.AddWithValue("$messageId", payload.Id.ToString());
                insertDeliveries.Parameters.AddWithValue("$topic", payload.Topic);
                insertDeliveries.ExecuteNonQuery();
            }

            transaction.Commit();
        }
    }

    public List<PayLoad> GetPendingDeliveries(string clientId)
    {
        var result = new List<PayLoad>();

        lock (_lock)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText =
                @"SELECT m.Id, m.Topic, m.Message, m.CreatedAtUtc
                  FROM deliveries d
                  JOIN messages m ON m.Id = d.MessageId
                  WHERE d.ClientId = $clientId AND d.Status = 0;";
            cmd.Parameters.AddWithValue("$clientId", clientId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new PayLoad
                {
                    Id = Guid.Parse(reader.GetString(0)),
                    Topic = reader.GetString(1),
                    Message = reader.GetString(2),
                    CreatedAtUtc = DateTime.Parse(reader.GetString(3))
                });
            }
        }

        return result;
    }

    public void RemovePendingDeliveries(string clientId, string topic)
    {
        lock (_lock)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText =
                @"DELETE FROM deliveries
                  WHERE ClientId = $clientId AND Status = 0
                  AND MessageId IN (SELECT Id FROM messages WHERE Topic = $topic);";
            cmd.Parameters.AddWithValue("$clientId", clientId);
            cmd.Parameters.AddWithValue("$topic", topic);
            cmd.ExecuteNonQuery();
        }
    }

    public void MarkDelivered(Guid messageId, string clientId)
    {
        lock (_lock)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText =
                "UPDATE deliveries SET Status = 1 WHERE MessageId = $messageId AND ClientId = $clientId;";
            cmd.Parameters.AddWithValue("$messageId", messageId.ToString());
            cmd.Parameters.AddWithValue("$clientId", clientId);
            cmd.ExecuteNonQuery();
        }
    }
}