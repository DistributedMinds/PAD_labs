using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicAPI.Repositories;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMongoRepository<Song> _songRepository;
        public MusicController(IMongoRepository<Song> songRepository)
        {
            _songRepository = songRepository;
            
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
            //de mutatat faza cu timpul in serviciu separat
            song.LastChangedAt = DateTime.UtcNow;
            var result = _songRepository.InsertRecord(song);
            
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Upsert(Song song)
        {
            //Treb si verificarea asta de mutat in serviciu separat
            if (song.Id == Guid.Empty)
            {
                return BadRequest("Empty song id");
            }
            song.LastChangedAt = DateTime.UtcNow;
            _songRepository.UpsertRecord(song);
            return Ok(song);
            
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
