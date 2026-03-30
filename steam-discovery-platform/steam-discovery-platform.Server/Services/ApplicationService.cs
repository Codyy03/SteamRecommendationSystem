using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;
namespace steam_discovery_platform.Server.Services
{
    public class ApplicationService : IApplicationService
    {
        readonly SteamDbContext context;
        
        public ApplicationService(SteamDbContext context)
        {
            this.context = context;
        }

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

            return gameDetailsDTO;
        }

        public async Task<List<GameInfoDTO>> GetGamesByGenreAsync(int count, string genre)
        {
            List<GameInfoDTO> gameInfoDTOs = await context.Applications.Include(a => a.Genres)
               .Where(a => a.Type == "game" && a.Genres.Any(g => g.Name == genre) && a.RecommendationsTotal > 10000).Select(
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

        public async Task<List<GameInfoDTO>> GetTopGamesAsync(int count)
        {
            List<GameInfoDTO> gameIinfoDTOs = await context.Applications.Where(a => a.Type == "game" && a.RecommendationsTotal > 10000).Select(
               a => new GameInfoDTO
               {
                   Appid = a.Appid,
                   Name = a.Name,
                   Type = a.Type,
                   HeaderImage = a.HeaderImage,
               }).Take(count)
               .OrderBy(a => Guid.NewGuid())
               .ToListAsync();

            return gameIinfoDTOs;
        }
    }
}
