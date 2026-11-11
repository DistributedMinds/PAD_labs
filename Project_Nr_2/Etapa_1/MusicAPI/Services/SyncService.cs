using Common.Models;
using Common.Utilities;
using MusicAPI.Settings;
using System.Text.Json;

namespace MusicAPI.Services
{
   
    public class SyncService<T> : ISyncService<T> where T : MongoDocument
    {
        private readonly ISyncServiceSettings _settings;
        private readonly string _origin;
        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);
        public SyncService(ISyncServiceSettings settings, IHttpContextAccessor httpContext) 
        {
            _settings = settings;
            _origin = httpContext.HttpContext.Request.Host.ToString();
        }    
        public HttpResponseMessage Delete(T record)
        {
            var syncType = _settings.DeleteHttpMethod;
            var json = ToSyncEntityJson(record, syncType);

            var response = HttpClientUtility.SendJson(json, _settings.Host, "POST");

            return response;
        }

        public List<SyncEntity> GetAllPending()
        {
            try
            {
                var response = HttpClientUtility.Get(_settings.Host);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<SyncEntity>();
                }

                var json = response.Content.ReadAsStringAsync().Result;
                
                return JsonSerializer.Deserialize<List<SyncEntity>>(json, _json) ?? new List<SyncEntity>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while trying to get all pending records: " + ex.Message);
                return new List<SyncEntity>();
            }
        }

        public bool TryGetPending(Guid id, out T record, out bool deleted)
        {
            record = null;
            deleted = false;
            try
            {
                var response = HttpClientUtility.Get($"{_settings.Host}/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var json = response.Content.ReadAsStringAsync().Result;
                var entity = JsonSerializer.Deserialize<SyncEntity>(json, _json);

                deleted = string.Equals(entity.SyncType, _settings.DeleteHttpMethod, StringComparison.OrdinalIgnoreCase);

                record = JsonSerializer.Deserialize<T>(entity.JsonData, _json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while trying to get pending record: " + ex.Message);
                return false;
            }
        }

        public HttpResponseMessage Upsert(T record)
        {
            var syncType = _settings.UpsertHttpMethod;
            var json = ToSyncEntityJson(record, syncType);

            var response = HttpClientUtility.SendJson(json, _settings.Host, "POST");

            return response;
        }

        private string ToSyncEntityJson(T record, string syncType)
        {
            var objectType = typeof(T);

            var syncEntity = new SyncEntity()
            {
                JsonData = JsonSerializer.Serialize(record),
                SyncType = syncType,
                ObjectType = objectType == typeof(Song) ? "music" : objectType.Name.ToLowerInvariant(),
                Id = record.Id,
                LastChangedAt = record.LastChangedAt,
                Origin = _origin
            };
            var json = JsonSerializer.Serialize(syncEntity);
            return json;
        }
    }
}
