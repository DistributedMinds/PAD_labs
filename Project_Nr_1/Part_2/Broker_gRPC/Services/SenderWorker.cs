using Broker;
using Grpc.Net.Client;
using GrpcAgent;

namespace Broker_gRPC.Services
{
    public class SenderWorker : IHostedService
    {
        private Timer _timer;
        private const int TimeToWait = 2000;
        private readonly IPersistentStore _persistentStore;
        public SenderWorker(IServiceScopeFactory serviceScopeFactory)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                _persistentStore = scope.ServiceProvider.GetRequiredService<IPersistentStore>();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoSendWork, null, 0, TimeToWait);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void DoSendWork(object? state)
        {
        }
    }
}
