using Microsoft.AspNetCore.Mvc;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;

namespace steam_discovery_platform.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PythonRecommendationController : ControllerBase
    {
        readonly IPythonRecommendationService pythonRecommendationService;

        public PythonRecommendationController(IPythonRecommendationService pythonRecommendationService)
        {
            this.pythonRecommendationService = pythonRecommendationService;
        }

        [HttpGet("pythonRecommendation")]
        public async Task <ActionResult<List<GameInfoDTO>>> GetPythonRecommendationGames(string gameName,float genre, float met,float pop, int howManyGames)
        {
            var games = await pythonRecommendationService.GetRecommendationsAsync(gameName, genre, met, pop, howManyGames);

            return games == null ? NotFound() : Ok(games);
        }

    }
}
