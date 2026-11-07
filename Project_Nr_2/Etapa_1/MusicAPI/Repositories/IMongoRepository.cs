using Common.Models;

namespace MusicAPI.Repositories;

public interface IMongoRepository<T>
{
    List<T> GetAllRecords();
    T InsertRecord(T record);
    T GetRecordById(Guid id);
    void UpsertRecord(T record);
    void DeleteRecord(Guid id);
    
    
}