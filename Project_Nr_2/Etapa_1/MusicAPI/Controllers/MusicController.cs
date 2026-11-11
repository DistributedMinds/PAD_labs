using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicAPI.Repositories;
using MusicAPI.Services;
using MusicAPI.Settings;
using System.Text.Json;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMongoRepository<Song> _songRepository;
        private readonly ISyncService<Song> _songSyncService;
        private readonly ISyncServiceSettings _settings;
        private readonly ITimeService _timeService;
        private readonly IGuidService _guidService;
        public MusicController(IMongoRepository<Song> songRepository, ISyncService<Song> songSyncService, ISyncServiceSettings settings, ITimeService timeService, IGuidService guidService)
        {
            _songRepository = songRepository;
            _songSyncService = songSyncService;
            _settings = settings;
            _timeService = timeService;
            _guidService = guidService;
        }
        [HttpGet]
        public List<Song> ListAllSongs()
        {
            //var records = _songRepository.GetAllRecords();
            var records = _songRepository.GetAllRecords().ToDictionary(s => s.Id);

            var pending = _songSyncService.GetAllPending().Where(e => e.ObjectType == "music");

            foreach (var entity in pending)
            {
                if (string.Equals(entity.SyncType, _settings.DeleteHttpMethod, StringComparison.OrdinalIgnoreCase))
                {
                    records.Remove(entity.Id);
                }
                else
                {
                    records[entity.Id] = JsonSerializer.Deserialize<Song>(entity.JsonData);
                }
            }
            return records.Values.ToList();
            //return records;
        }

        [HttpGet("{id}")]
        public Song GetSongById(string id)
        {
            var guid = Guid.Parse(id);

            if (_songSyncService.TryGetPending(guid, out var pending, out var deleted))
            {
                return deleted ? null : pending;
            }

            var result = _songRepository.GetRecordById(Guid.Parse(id));
            return result;
        }

        [HttpPost]

        public IActionResult CreateSong(Song song)
        {
            var songEntity = _timeService.SetRecordTime(song);
            var result = _songRepository.InsertRecord(songEntity);

            _songSyncService.Upsert(songEntity);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Upsert(Song song)
        {
            if (_guidService.IsEmpty(song.Id))
            {
                return BadRequest();
            }
            var songEntity = _timeService.SetRecordTime(song);
            _songRepository.UpsertRecord(songEntity);

            _songSyncService.Upsert(songEntity);

            return Ok(songEntity);
            
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSong(string id)
        {
            var song = _songRepository.GetRecordById(Guid.Parse(id));

            if (song == null)
            {
                return BadRequest("Song not found");
            }
            _songRepository.DeleteRecord(Guid.Parse(id));

            song = _timeService.SetRecordTime(song);
            _songSyncService.Delete(song);

            return Ok("Song deleted "+id);
        }

        [HttpPut("sync")]
        public IActionResult UpsertSync(Song song)
        {
            var existingSong = _songRepository.GetRecordById(song.Id);

            if(existingSong == null || song.LastChangedAt > existingSong.LastChangedAt)
            {
                _songRepository.UpsertRecord(song);
            }

            return Ok();
        }

        [HttpDelete("sync")]
        public IActionResult DeleteSync(Song song)
        {
            var existingSong = _songRepository.GetRecordById(song.Id);

            if (existingSong != null || song.LastChangedAt > existingSong.LastChangedAt)
            {
                _songRepository.DeleteRecord(song.Id);
            }

            return Ok();
        }
    }
}
