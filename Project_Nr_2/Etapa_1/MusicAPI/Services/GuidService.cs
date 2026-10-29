
namespace MusicAPI.Services;

public class GuidService : IGuidService
{
    public bool IsEmpty(Guid id)
    {
        if (id == Guid.Empty)
        {
            return true;
        }
        return false;
    }
}