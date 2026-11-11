using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyncNode.Services;

namespace SyncNode.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly SyncWorkJobService _workJobService;
        public SyncController(SyncWorkJobService workJobService)
        {
            _workJobService = workJobService;
        }

        [HttpPost]
        public IActionResult Sync(SyncEntity entity)
        {
            _workJobService.Additem(entity);
            return Ok();
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetPending(Guid id)
        {
            return _workJobService.TryGetPending(id, out var entity) ? Ok(entity) : NotFound();
        }

        [HttpGet]
        public IActionResult ListAllPending()
        {
            return Ok(_workJobService.GetAllPending());
        }
    }
}
