using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicAPI.Repositories;
using MusicAPI.Services;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMongoRepository<Song> _songRepository;
        private readonly ITimeService _timeService;
        private readonly IGuidService _guidService;
        public MusicController(IMongoRepository<Song> songRepository,  ITimeService timeService, IGuidService guidService)
        {
            _songRepository = songRepository;
            _timeService = timeService;
            _guidService = guidService;
        }
        [HttpGet]
        public List<Song> ListAllSongs()
        {
            var records = _songRepository.GetAllRecords();
            return records;
        }

        [HttpGet("{id}")]
        public Song GetSongById(string id)
        {
            var result = _songRepository.GetRecordById(Guid.Parse(id));
            return result;
        }

        [HttpPost]

        public IActionResult CreateSong(Song song)
        {
            var songEntity = _timeService.SetRecordTime(song);
            var result = _songRepository.InsertRecord(songEntity);
            
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
            
            return Ok("Song deleted "+id);
        }
    }
}
