using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Scalesoft.DisplayTool.Service.Models;

namespace Scalesoft.DisplayTool.Service.Controllers;

[Route("")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> m_logger;

    public HomeController(ILogger<HomeController> logger)
    {
        m_logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return Redirect("/swagger");
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return StatusCode(StatusCodes.Status500InternalServerError, new GenericErrorContract
        {
            Title = "Internal server error.",
            Status = StatusCodes.Status500InternalServerError,
            TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
        });
    }
}