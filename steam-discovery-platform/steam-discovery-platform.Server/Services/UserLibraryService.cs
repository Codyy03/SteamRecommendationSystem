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

        /// <inheritdoc />
        public async Task ChangeGameFavoriteStatus(int appid, Guid userId, bool isFavorite)
        {
            var userLibraryGame = await context.UserLibraries
                .FirstOrDefaultAsync(us => us.Appid == appid && us.UserId == userId);

            if (userLibraryGame != null)
            {
                userLibraryGame.IsFavorite = isFavorite;
                await context.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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
                              },
                              Genres = string.Join(", ", app.Genres.Select(g => g.Name))
                          })
                    .ToListAsync();
        }

        /// <inheritdoc />
        public async Task RemoveGameFromLibrary(int appid, Guid userId)
        {
            var gameInLibrary = await context.UserLibraries
                .Where(ul => ul.UserId == userId && ul.Appid == appid)
                .FirstOrDefaultAsync();

            if (gameInLibrary == null) return;

            context.Remove(gameInLibrary);
            await context.SaveChangesAsync();
        }
    }
}
