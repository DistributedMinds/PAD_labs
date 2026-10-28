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
    }
}
