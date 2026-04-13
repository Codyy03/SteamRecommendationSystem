using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;
using System.Globalization;

namespace steam_discovery_platform.Server.Services
{
    /// <summary>
    /// Service that communicates with an external Python FastAPI/Flask server to fetch AI-driven recommendations.
    /// </summary>
    public class PythonRecommendationService : IPythonRecommendationService
    {
        readonly HttpClient httpClient;
        readonly SteamdbContext context;
        public PythonRecommendationService(HttpClient httpClient, IApplicationService appService, SteamdbContext context)
        {
            this.httpClient = httpClient;
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<PythonRecommendationResponse> GetRecommendationsAsync(string query, float genre = 0.4f, float met = 0.3f, float pop = 0.15f, int howManyGames = 20, float series_penalty = 0.4f)
        {
            // ask python for recomendations
            var response = await httpClient.GetAsync(
                string.Create(CultureInfo.InvariantCulture,
                $"http://localhost:8000/recommend?query={query}&genre_weight={genre}&meta_weight={met}&pop_weight={pop}&how_many_games={howManyGames}&series_penalty_value={series_penalty}")
            );

            if (!response.IsSuccessStatusCode) return new PythonRecommendationResponse();

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

            pythonData.Recommendations = games.OrderBy(g => ids.IndexOf(g.Appid)).ToList();

            return pythonData;
        }

        /// <inheritdoc />
        public async Task<PythonRecommendationResponse> GetUserRecommendationsAsync(string query, float genre, float met, float pop, int howManyGames)
        {
            var encodedQuery = Uri.EscapeDataString(query);

            // ask python for recomendations
            var response = await httpClient.GetAsync(
             string.Create(CultureInfo.InvariantCulture,
             $"http://localhost:8000/user_recommend?game_names={encodedQuery}&genre_weight={genre}&meta_weight={met}&pop_weight={pop}&how_many_games={howManyGames}")
         );

            if (!response.IsSuccessStatusCode) return new PythonRecommendationResponse();

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

            pythonData.Recommendations = games.OrderBy(g => ids.IndexOf(g.Appid)).ToList();

            return pythonData;
        }
    }
}
