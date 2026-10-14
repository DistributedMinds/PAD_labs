namespace Broker_gRPC.Storage;

public interface ISessionStore
{
    string Create(string username);
    string? GetUsername(string token);
    void Remove(string token);
}