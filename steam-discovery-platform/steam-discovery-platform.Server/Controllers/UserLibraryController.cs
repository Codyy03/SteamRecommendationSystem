using Microsoft.AspNetCore.Mvc;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;

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

        [HttpPost("addGameToLibrary")]
        public async Task<NoContentResult> AddGameToUserLibrary([FromBody] UserGameDTO userGameDTO)
        {
            var newUser = await userLibraryService.AddGameToLibrary(userGameDTO);

            return NoContent();
        }
    }
}
