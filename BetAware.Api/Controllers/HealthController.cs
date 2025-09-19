using Microsoft.AspNetCore.Mvc;

namespace BetAware.Api.Controllers;

[ApiController]
[Route("v1/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult HealthCheck()
    {
        return Ok("BetAware API está online");
    }
}
