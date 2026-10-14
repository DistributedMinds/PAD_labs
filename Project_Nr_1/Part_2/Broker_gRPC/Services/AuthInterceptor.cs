using Broker_gRPC.Storage;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Broker_gRPC.Services;

public class AuthInterceptor : Interceptor
{
    public const string UserKey = "username";

    private const string SubscriberPrefix = "/GrpcAgent.Subscriber/";
    private const string BearerPrefix = "Bearer ";

    private readonly ISessionStore _sessions;

    public AuthInterceptor(ISessionStore sessions)
    {
        _sessions = sessions;
    }

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        // Doar Subscribe/Unsubscribe cer autentificare (ca în Part 1)
        if (context.Method.StartsWith(SubscriberPrefix, StringComparison.Ordinal))
        {
            var header = context.RequestHeaders.GetValue("authorization");

            string? token = header != null &&
                            header.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase)
                ? header.Substring(BearerPrefix.Length)
                : null;

            string? username = token == null ? null : _sessions.GetUsername(token);
            if (username == null)
            {
                Console.WriteLine($"[AUTH] header='{header}', token='{token}'");
                throw new RpcException(new Status(StatusCode.Unauthenticated, "not authenticated"));
            }

            context.UserState[UserKey] = username;
        }

        return continuation(request, context);
    }
}