using Common.Models;

namespace MusicAPI.Services;

public interface ITimeService
{
    Song SetRecordTime(Song song);
}