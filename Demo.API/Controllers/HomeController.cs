using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Demo.Api Service is running");
    }
}
