using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Services
{
    /// <summary>
    /// Service responsible for managing and retrieving steam application data.
    /// </summary>
    public class ApplicationService : IApplicationService
    {
        readonly SteamdbContext context;
        
        public ApplicationService(SteamdbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves detailed information about a specific game, including developers, publishers, genres, and categories.
        /// </summary>
        /// <param name="id">The unique Steam Application ID (Appid).</param>
        /// <returns>A DTO containing comprehensive game details, or null if not found.</returns>
        public async Task<GameDetailsDTO> GetGameDetails(int id)
        {
            GameDetailsDTO? gameDetailsDTO = await context.Applications
                .Include(a => a.Developers)
                .Include(a => a.Categories)
                .Include(a => a.Publishers)
                .Include(a => a.Genres)
                .Where(a => a.Appid == id).
                Select(
                a => new GameDetailsDTO
                {
                    Appid = a.Appid,
                    Name = a.Name,
                    Type = a.Type,
                    IsFree = a.IsFree,
                    RecommendationsTotal = a.RecommendationsTotal,
                    ReleaseDate = a.ReleaseDate,
                    ShortDescription = a.ShortDescription,
                    HeaderImage = a.HeaderImage,
                    MetacriticScore = a.MetacriticScore,
                    FinalPrice = a.FinalPrice,
                    Currency = a.Currency,
                    SupportsLinux = a.SupportsLinux,
                    SupportsMac = a.SupportsMac,
                    SupportsWindows = a.SupportsWindows,
                    PcRequirements = a.PcRequirements,
                    CreatedAt = a.CreatedAt,
                    Developers = string.Join(", ", a.Developers.Select(d => d.Name)),
                    Publishers = string.Join(", ", a.Publishers.Select(p => p.Name)),
                    Categories = string.Join(", ", a.Categories.Select(c => c.Name)),
                    Genres = string.Join(", ", a.Genres.Select(g => g.Name))
                }).FirstOrDefaultAsync();

            return gameDetailsDTO!;
        }

        /// <summary>
        /// Retrieves a random collection of games filtered by a specific genre and minimum popularity threshold.
        /// </summary>
        /// <param name="count">The maximum number of games to return.</param>
        /// <param name="genre">The name of the genre to filter by (case-insensitive).</param>
        /// <returns>A list of games matching the specified genre.</returns>
        public async Task<List<GameInfoDTO>> GetGamesByGenreAsync(int count, string genre)
        {
            List<GameInfoDTO> gameInfoDTOs = await context.Applications.Include(a => a.Genres)
               .Where(a => a.Type == "game" && a.Genres.Any(g => g.Name.ToLower() == genre.ToLower()) && a.RecommendationsTotal > 10000).Select(
               a => new GameInfoDTO
               {
                   Appid = a.Appid,
                   Name = a.Name,
                   Type = a.Type,
                   HeaderImage = a.HeaderImage,
               }).OrderBy(a => Guid.NewGuid())
               .Take(count)
               .ToListAsync();

            return gameInfoDTOs;
        }

        /// <summary>
        /// Retrieves a random selection of top-rated/popular games.
        /// </summary>
        /// <param name="count">The maximum number of games to return.</param>
        /// <returns>A list of high-recommendation games.</returns>
        public async Task<List<GameInfoDTO>> GetTopGamesAsync(int count)
        {
            List<GameInfoDTO> gameIinfoDTOs = await context.Applications.Where(a => a.Type == "game" && a.RecommendationsTotal > 10000).Select(
               a => new GameInfoDTO
               {
                   Appid = a.Appid,
                   Name = a.Name,
                   Type = a.Type,
                   HeaderImage = a.HeaderImage,
               })
               .OrderBy(a => Guid.NewGuid())
               .Take(count)
               .ToListAsync();

            return gameIinfoDTOs;
        }
    }
}
