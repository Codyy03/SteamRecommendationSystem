using Microsoft.EntityFrameworkCore;
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

        public async Task<List<UserLibraryGameDTO>> GetUserLibraryGames(Guid userId)
        {
            return await context.UserLibraries
             .Where(ul => ul.UserId == userId)
             .Join(context.Applications,
                   ul => ul.Appid,
                   app => app.Appid,
                   (ul, app) => new UserLibraryGameDTO
                   {
                       IsFavorite = ul.IsFavorite,
                       AddedAt = ul.AddedAt,
                       Game = new GameInfoDTO 
                       {
                           Appid = app.Appid,
                           Name = app.Name,
                           Type = app.Type,
                           HeaderImage = app.HeaderImage
                       }
                   })
             .ToListAsync();
        }
    }
}
