using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        readonly SteamDbContext context;

        public ApplicationsController(SteamDbContext context)
        {
            this.context = context;
        }

        [HttpGet(Name ="getGames")]
        public async Task<ActionResult<List<GameInfoDTO>>> GetGamesInfo()
        {
            List<GameInfoDTO> gameIinfoDTOs = await context.Applications.Select(
                a => new GameInfoDTO
                {
                    Appid = a.Appid,
                    Name = a.Name,
                    Type = a.Type,
                }).Take(10).ToListAsync();

            if (gameIinfoDTOs == null)
                return NotFound();

            return Ok(gameIinfoDTOs);
        }

        public class GameInfoDTO
        {
            public int Appid { get; set; }

            public string Name { get; set; } = null!;

            public string? Type { get; set; }
        }
    }
}
