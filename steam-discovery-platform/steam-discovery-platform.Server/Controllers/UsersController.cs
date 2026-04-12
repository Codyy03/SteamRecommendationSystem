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

        /// <summary>
        /// Retrieves basic information about a specific user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique GUID of the user.</param>
        /// <returns>A DTO containing user profile data.</returns>
        [HttpGet("getUser")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            return Ok(await userService.GetUser(id));
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userRegisterDTO">The registration details including username, email, and password.</param>
        /// <returns>The newly created user data and the location of the resource.</returns>
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

        /// <summary>
        /// Authenticates a user and returns a session token or login result.
        /// </summary>
        /// <param name="loginDTO">Credentials (email/username and password).</param>
        /// <returns>A login response containing authentication details.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await userService.Login(loginDTO);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the profile information of the currently authenticated user based on their claims.
        /// </summary>
        /// <returns>The current user's profile data.</returns>
        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> GetMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Unauthorized();

            var userDto = await userService.GetMe(Guid.Parse(userId));
            return Ok(userDto);
        }

        /// <summary>
        /// Updates the profile details (e.g., username or email) of the currently authenticated user.
        /// </summary>
        /// <param name="updateUserDTO">The updated information for the user profile.</param>
        /// <returns>The updated user profile data.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            updateUserDTO.UserId = new Guid(userId);
            var userDto = await userService.UpdateUser(updateUserDTO);

            return Ok(userDto);
        }

        /// <summary>
        /// Securely resets or changes the password for the currently authenticated user.
        /// </summary>
        /// <param name="changePasswordDto">Object containing the old password and the new password.</param>
        /// <returns>204 No Content on successful password update.</returns>
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
