using Microsoft.AspNetCore.Mvc;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using System.Security.Claims;

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
            return Ok(await userService.GetUser(id));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var newUser = await userService.CreateUser(userRegisterDTO);

            var response = new UserDTO
            {
                Id = newUser.Id,
                CreatedAt = newUser.CreatedAt,
                Username = newUser.Username,
                Email = newUser.Email,
            };

            return CreatedAtAction(nameof(GetUser), new { id = response.Id }, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await userService.Login(loginDTO);
            return Ok(result);
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> GetMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Unauthorized();

            var userDto = await userService.GetMe(Guid.Parse(userId));
            return Ok(userDto);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            updateUserDTO.UserId = new Guid(userId);
            var userDto = await userService.UpdateUser(updateUserDTO);

            return Ok(userDto);
        }

        [HttpPut("passwordReset")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await userService.ChangePassword(Guid.Parse(userId), changePasswordDto);

            return NoContent();
        }
    }
}
