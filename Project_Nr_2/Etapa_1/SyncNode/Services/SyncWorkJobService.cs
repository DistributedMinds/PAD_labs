using Common.Models;
using System.Collections.Concurrent;

namespace SyncNode.Services
{
    public class SyncWorkJobService :IHostedService
    {
        private readonly ConcurrentDictionary<Guid, SyncEntity> documents = 
            new ConcurrentDictionary<Guid, SyncEntity>();

        private Timer _timer;

        public void Additem(SyncEntity entity)
        {
            SyncEntity document = null;
            bool isPresent = documents.TryGetValue(entity.Id, out document);

            //verificarea ca ultima modificare este prioritara
            if (!isPresent || (isPresent && entity.LastChangedAt > document.LastChangedAt ))
            {
                documents[entity.Id] = entity;
            }
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoSendWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));

            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void  DoSendWork(object state)
        {

        }
    }
}
