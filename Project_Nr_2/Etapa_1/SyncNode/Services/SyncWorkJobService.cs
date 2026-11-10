using Common.Models;
using Common.Utilities;
using SyncNode.Settings;
using System.Collections.Concurrent;

namespace SyncNode.Services
{
    public class SyncWorkJobService :IHostedService
    {
        private readonly ConcurrentDictionary<Guid, SyncEntity> documents = 
            new ConcurrentDictionary<Guid, SyncEntity>();

        private readonly IMusicAPISettings _settings;

        private Timer _timer;

        public SyncWorkJobService(IMusicAPISettings settings)
        {
            _settings = settings;
        }


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

        private void DoSendWork(object state)
        {
            foreach (var doc in documents)
            {
                SyncEntity entity = null;
                var isPresent = documents.TryRemove(doc.Key, out entity);

                if (isPresent)
                {
                    var receivers = _settings.Hosts.Where(x => !x.Contains(entity.Origin));

                    foreach (var receiver in receivers)
                    {
                        // http://localhost:9001/song/sync
                        // song -> music
                        var url = $"{receiver}/{entity.ObjectType}/sync";

                        try
                        {
                            var result = HttpClientUtility.SendJson(entity.JsonData, url, entity.SyncType);

                            if (!result.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"Sync failed: {url}, StatusCode: {result.StatusCode}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Could not send sync request to {url}: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}
