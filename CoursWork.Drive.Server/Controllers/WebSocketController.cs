using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursWork.Drive.Server.Controllers
{
    [Route("/ws"), Authorize(Roles = "Admin, User"), ApiController]
    public class WebSocketController : ControllerBase 
    {
        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
