using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SyncNode.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        [HttpPost]
        public IActionResult Sync(SyncEntity entity)
        {
            return Ok();
        }
        

    }
}
