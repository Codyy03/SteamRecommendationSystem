using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user, validates data uniqueness (email/username), and hashes the password.
        /// </summary>
        public Task<User> CreateUser(UserRegisterDTO userRegisterDTO);

        /// <summary>
        /// Updates user profile information such as username and email with validation checks.
        /// </summary>
        public Task<UserDTO> UpdateUser(UpdateUserDTO updateUserDTO);

        /// <summary>
        /// Finds a user by their unique identifier.
        /// </summary>
        public Task<UserDTO> GetUser(Guid id);

        /// <summary>
        /// Validates credentials, generates a JWT Access Token and a Refresh Token, and cleans up old sessions.
        /// </summary>
        public Task<LoginResponseDTO> Login(LoginDTO loginDTO);

        /// <summary>
        /// Retrieves the profile of the currently logged-in user.
        /// </summary>
        public Task<UserDTO> GetMe(Guid userId);

        /// <summary>
        /// Updates the password for a specific user after validating the new password strength.
        /// </summary>
        public Task ChangePassword(Guid userId, ChangePasswordDto changePasswordDto);
    }
}
