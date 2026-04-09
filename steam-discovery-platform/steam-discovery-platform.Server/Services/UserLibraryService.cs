using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Helpers;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Services
{
    public class UserLibraryService : IUserLibraryService
    {
        readonly SteamdbContext context;
        readonly JwtTokenHelper jwtHelper;

        public UserLibraryService(SteamdbContext context, JwtTokenHelper jwtHelper)
        {
            this.context = context;
            this.jwtHelper = jwtHelper;
        }

        public async Task<UserLibrary> AddGameToLibrary(Guid userID, UserGameDTO userGameDTO)
        {
            UserLibrary gameDTO = new UserLibrary
            {
                UserId = userID,
                Appid = userGameDTO.Appid,
                IsFavorite = false,
                AddedAt = DateTime.UtcNow,
            };

            context.Add(gameDTO);
            await context.SaveChangesAsync();

            return gameDTO;
        }
    }
}
