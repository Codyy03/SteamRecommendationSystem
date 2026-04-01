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
        readonly SteamDbContext context;
        public PythonRecommendationService(HttpClient httpClient, IApplicationService appService, SteamDbContext context)
        {
            this.httpClient = httpClient;
            this.context = context;
        }

        /// <summary>
        /// Calls the Python recommendation engine and maps the resulting IDs to full game database records.
        /// </summary>
        /// <param name="query">The search term or game name to base recommendations on.</param>
        /// <param name="genre">Influence of genre similarity on the result (default 0.4).</param>
        /// <param name="met">Influence of Metacritic score on the result (default 0.3).</param>
        /// <param name="pop">Influence of popularity on the result (default 0.15).</param>
        /// <param name="howManyGames">Total number of results requested (default 20).</param>
        /// <returns>A list of ordered <see cref="GameInfoDTO"/> matching the Python engine's rankings.</returns>
        public async Task<List<GameInfoDTO>> GetRecommendationsAsync(string query, float genre = 0.4f, float met = 0.3f, float pop = 0.15f, int howManyGames = 20, float series_penalty = 0.4f)
        {
            // ask python for recomendations
            var response = await httpClient.GetAsync(
                string.Create(CultureInfo.InvariantCulture,
                $"http://localhost:8000/recommend?query={query}&genre_weight={genre}&meta_weight={met}&pop_weight={pop}&how_many_games={howManyGames}&series_penalty_value={series_penalty}")
            );

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
