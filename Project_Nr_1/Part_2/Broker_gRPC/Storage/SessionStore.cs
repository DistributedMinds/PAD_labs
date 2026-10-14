using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace Broker_gRPC.Storage;

public class SessionStore : ISessionStore
{
    private readonly ConcurrentDictionary<string, string> _sessions = new();

    public string Create(string username)
    {
        string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        _sessions[token] = username;
        return token;
    }

    public string? GetUsername(string token)
        => _sessions.TryGetValue(token, out var username) ? username : null;

    public void Remove(string token)
        => _sessions.TryRemove(token, out _);
}