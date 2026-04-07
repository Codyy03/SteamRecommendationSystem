using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Services;
using steam_discovery_platform.Server.Validation;
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
            try
            {
                return Ok(await userService.GetUser(id));
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserRegisterDTO userRegisterDTO)
        {
            try
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
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var result = await userService.Login(loginDTO);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> GetMe()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized();

                var userDto = await userService.GetMe(Guid.Parse(userId));
                return Ok(userDto);
            }
            catch (ValidationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized();

                updateUserDTO.UserId = new Guid(userId);
                var userDto = await userService.UpdateUser(updateUserDTO);

                return Ok(userDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
