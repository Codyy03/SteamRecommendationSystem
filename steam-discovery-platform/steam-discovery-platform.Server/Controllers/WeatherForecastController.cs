using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        readonly SteamDbContext context;

        public WeatherForecastController(SteamDbContext context)
        {
            this.context = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var games = await context.Applications.Take(10).ToListAsync();

            return Ok(games);
        }
    }
}
