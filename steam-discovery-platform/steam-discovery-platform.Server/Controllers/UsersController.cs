using Microsoft.AspNetCore.Mvc;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;

namespace steam_discovery_platform.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("getUser")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            var newUser = await userService.GetUser(id);

            return newUser == null ? NotFound() : newUser;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var newUser = await userService.CreateUser(userRegisterDTO);

            var response = new UserDTO
            {
                Id = newUser.Id,
                CreatedAt = DateTime.Now,
                Username = newUser.Username,
                Email = newUser.Email,
            };

            return CreatedAtAction(nameof(GetUser), new { id = response.Id }, response);
        }
    }
}
