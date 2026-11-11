

using Common.Models;
using MongoDB.Driver;

namespace MusicAPI.Services
{
    public interface ISyncService<T> where T : MongoDocument
    {
        HttpResponseMessage Upsert(T record);

        HttpResponseMessage Delete(T record);

        bool TryGetPending(Guid id, out T record, out bool deleted);

        List<SyncEntity> GetAllPending();
    }
}
