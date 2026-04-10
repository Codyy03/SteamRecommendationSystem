using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Interfaces
{
    public interface IUserLibraryService
    {
        public Task<UserLibrary> AddGameToLibrary(Guid userId, UserGameDTO userGameDTO);
        public Task<List<UserLibraryGameDTO>> GetUserLibraryGames(Guid userId);
        public Task RemoveGameFromLibrary(int appid, Guid userId);
    }
}
