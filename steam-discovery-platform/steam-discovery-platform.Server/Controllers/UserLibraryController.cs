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

        [Authorize]
        [HttpGet("userLibrary")]
        public async Task<ActionResult<List<UserLibraryGameDTO>>> GetUserLibraryGames()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var library = await userLibraryService.GetUserLibraryGames(Guid.Parse(userIdString));
            return library;
        }

        [HttpDelete("/api/usersLibrary/delete/{appid}")]
        public async Task<IActionResult> RemoveGameFromLibrary([FromRoute] int appid)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            await userLibraryService.RemoveGameFromLibrary(appid, Guid.Parse(userIdString));
            return NoContent();
        }

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
