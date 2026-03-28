using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Services
{
    public class PythonRecommendationService : IPythonRecommendationService
    {
        readonly HttpClient httpClient;
        readonly SteamDbContext context;
        public PythonRecommendationService(HttpClient httpClient, IApplicationService appService, SteamDbContext context)
        {
            this.httpClient = httpClient;
            this.context = context;
        }

        public async Task<List<GameInfoDTO>> GetRecommendationsAsync(string query)
        {
            // ask python for recomendations
            var response = await httpClient.GetAsync($"http://localhost:8000/recommend?query={query}");

            if (!response.IsSuccessStatusCode) return new List<GameInfoDTO>();

            var pythonData = await response.Content.ReadFromJsonAsync<PythonRecommendationResponse>();

            var ids = pythonData.Recommendations.Select(r => r.Appid).ToList();

            var games = await context.Applications.Where(a => ids.Contains(a.Appid))
                .Select(a => new GameInfoDTO
                {
                    Appid = a.Appid,
                    Name = a.Name,
                    HeaderImage = a.HeaderImage,
                    Type = a.Type,
                }).ToListAsync();

            return games.OrderBy(g => ids.IndexOf(g.Appid)).ToList();
        }

    }
}
