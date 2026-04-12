using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using System.Security.Claims;

namespace steam_discovery_platform.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLibraryController : ControllerBase
    {
        readonly IUserLibraryService userLibraryService;

        public UserLibraryController(IUserLibraryService userLibraryService)
        {
            this.userLibraryService = userLibraryService;
        }

        /// <summary>
        /// Adds a specific game to the authorized user's personal library.
        /// </summary>
        /// <param name="userGameDTO">Object containing game ID and optional user notes.</param>
        /// <returns>204 No Content on success.</returns>
        [Authorize]
        [HttpPost("addGameToLibrary")]
        public async Task<IActionResult> AddGameToUserLibrary([FromBody] UserGameDTO userGameDTO)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User ID not found in token.");
            }

            if (Guid.TryParse(userIdString, out Guid userId))
            {
                await userLibraryService.AddGameToLibrary(userId, userGameDTO);
                return NoContent();
            }

            return BadRequest("Invalid User ID format in token.");
        }

        /// <summary>
        /// Retrieves all games currently stored in the authorized user's library.
        /// </summary>
        /// <returns>A list of games with their favorite status and added date.</returns>
        [Authorize]
        [HttpGet("userLibrary")]
        public async Task<ActionResult<List<UserLibraryGameDTO>>> GetUserLibraryGames()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var library = await userLibraryService.GetUserLibraryGames(Guid.Parse(userIdString));
            return library;
        }

        /// <summary>
        /// Removes a game from the user's library based on the provided Steam AppID.
        /// </summary>
        /// <param name="appid">The unique Steam Application ID.</param>
        /// <returns>204 No Content on success.</returns>
        [HttpDelete("/api/usersLibrary/delete/{appid}")]
        public async Task<IActionResult> RemoveGameFromLibrary([FromRoute] int appid)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            await userLibraryService.RemoveGameFromLibrary(appid, Guid.Parse(userIdString));
            return NoContent();
        }

        /// <summary>
        /// Toggles the 'Favorite' flag for a specific game in the user's library.
        /// </summary>
        /// <param name="appid">The unique Steam Application ID.</param>
        /// <param name="isFavorite">The new favorite status (true/false).</param>
        /// <returns>204 No Content on success.</returns>
        [HttpPut("/api/usersLibrary/updateFavoriteGame/{appid}")]
        public async Task<IActionResult> ChangeGameFavoriteStatus(int appid, [FromQuery] bool isFavorite)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            await userLibraryService.ChangeGameFavoriteStatus(appid, Guid.Parse(userIdString), isFavorite);
            return NoContent();
        }

    }
}
