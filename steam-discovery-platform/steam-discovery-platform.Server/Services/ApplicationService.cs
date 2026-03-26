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

        public async Task<List<GameInfoDTO>> GetTopGamesAsync(int count)
        {
            List<GameInfoDTO> gameIinfoDTOs = await context.Applications.Where(a => a.Type == "game" && a.RecommendationsTotal > 1000).Select(
               a => new GameInfoDTO
               {
                   Appid = a.Appid,
                   Name = a.Name,
                   Type = a.Type,
               }).Take(count).ToListAsync();

            return gameIinfoDTOs;
        }
    }
}
