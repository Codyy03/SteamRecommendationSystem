using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;
using System.Security.Claims;

namespace steam_discovery_platform.Server.Controllers
{
    /// <summary>
    /// API Controller for handling steam application requests.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        readonly IApplicationService appService;

        public ApplicationsController(IApplicationService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Gets a list of top popular games.
        /// </summary>
        /// <returns>An ActionResult containing a list of top games.</returns>
        [HttpGet("getGames")]
        public async Task<ActionResult<List<GameInfoDTO>>> GetGamesInfo()
        {
            var games = await appService.GetTopGamesAsync(10);

            return games == null ? NotFound() : Ok(games);
        }

        /// <summary>
        /// Gets a list of games filtered by a specific genre.
        /// </summary>
        /// <param name="genre">The genre name to search for.</param>
        /// <returns>An ActionResult containing a list of games matching the genre.</returns>
        [HttpGet("getGamesByGenre")]
        public async Task<ActionResult<List<GameInfoDTO>>> GetGamessByGenre(string genre)
        {
            var games = await appService.GetGamesByGenreAsync(10, genre);

            return games == null ? NotFound() : Ok(games);
        }

        /// <summary>
        /// Gets full details for a specific game by its ID.
        /// </summary>
        /// <param name="id">The unique Steam ID of the application.</param>
        /// <returns>An ActionResult containing the game details.</returns>
        [HttpGet("getGameDetails")]
        public async Task<ActionResult<GameDetailsDTO>> GetGameDetails(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Guid userId = string.IsNullOrEmpty(userIdClaim) ? Guid.Empty : Guid.Parse(userIdClaim);

            var gameDetails = await appService.GetGameDetails(id, userId);

            return gameDetails == null ? NotFound() : Ok(gameDetails);
        }
    }
}
