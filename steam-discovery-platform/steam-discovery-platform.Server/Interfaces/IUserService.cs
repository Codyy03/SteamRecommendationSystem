using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Interfaces
{
    public interface IUserService
    {
       public Task<User> CreateUser(UserRegisterDTO userRegisterDTO);
        public Task<UserDTO> GetUser(Guid id);
    }
}
