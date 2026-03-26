using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        readonly IApplicationService appService;

        public ApplicationsController(IApplicationService appService)
        {
            this.appService = appService;
        }

        [HttpGet("getGames")]
        public async Task<ActionResult<List<GameInfoDTO>>> GetGamesInfo()
        {
            var games = await appService.GetTopGamesAsync(10);

            return games == null ? NotFound() : Ok(games);
        }
    }
}
