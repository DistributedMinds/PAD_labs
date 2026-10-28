using Common.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MusicAPI.Settings;

namespace MusicAPI.Repositories;

public class MongoRepository<T> : IMongoRepository<T> where T : MongoDocument
{
   private readonly IMongoDatabase _db;
   private readonly IMongoCollection<T> _collection;
 
   public MongoRepository(IMongoDbSettings dbSettings)
   {
      _db = new MongoClient(dbSettings.ConnectionString).GetDatabase(dbSettings.DatabaseName);
      
      string collectionName = typeof(T).Name.ToLower();
      _collection = _db.GetCollection<T>(collectionName);
      
   }
   public List<T> GetAllRecords()
   {
      var records = _collection.Find(new BsonDocument()).ToList();
      
      return records;
   }

   public T InsertRecord(T record)
   {
      throw new NotImplementedException();
   }

   public T GetRecordById(Guid id)
   {
      throw new NotImplementedException();
   }

   public void UpsertRecord(T record)
   {
      throw new NotImplementedException();
   }

   public void DeleteRecord(Guid id)
   {
      throw new NotImplementedException();
   }
}