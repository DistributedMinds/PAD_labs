using Common.Models;

namespace MusicAPI.Services;

public class TimeService: ITimeService
{
    public Song SetRecordTime(Song song)
    {
        song.LastChangedAt = DateTime.Now;
        return song;
    }
}