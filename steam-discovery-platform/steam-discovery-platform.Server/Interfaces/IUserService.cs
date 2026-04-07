using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Interfaces
{
    public interface IUserService
    {
        public Task<User> CreateUser(UserRegisterDTO userRegisterDTO);
        public Task<UserDTO> UpdateUser(UpdateUserDTO updateUserDTO);
        public Task<UserDTO> GetUser(Guid id);
        public Task<LoginResponseDTO> Login(LoginDTO loginDTO);
        public Task<UserDTO> GetMe(Guid userId);
    }
}
