using Microsoft.AspNetCore.Mvc;

namespace YoApp.Backend.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        [HttpGet("VerificationOnline")]
        public IActionResult VerificationOnline()
        {
            return Ok();
        }
    }
}
