using steam_discovery_platform.Server.DTOs;

namespace steam_discovery_platform.Server.Interfaces
{
    public interface IPythonRecommendationService
    {
        /// <summary>
        /// Calls the Python recommendation engine and maps the resulting IDs to full game database records.
        /// </summary>
        /// <param name="query">The search term or game name to base recommendations on.</param>
        /// <param name="genre">Influence of genre similarity on the result (default 0.4).</param>
        /// <param name="met">Influence of Metacritic score on the result (default 0.3).</param>
        /// <param name="pop">Influence of popularity on the result (default 0.15).</param>
        /// <param name="howManyGames">Total number of results requested (default 20).</param>
        /// <returns>A list of ordered <see cref="GameInfoDTO"/> matching the Python engine's rankings.</returns>
        Task<PythonRecommendationResponse> GetRecommendationsAsync(string query, float genre, float met, float pop, int howManyGames, float series_penalty);

        /// <summary>
        /// Communicates with the external Python FastAPI service to get game recommendations.
        /// Sends user preferences (genre, metacritic, popularity weights) and returns enriched game data from the local database.
        /// </summary>
        /// <param name="query">Encoded string of games to base recommendations on.</param>
        /// <param name="genre">Weight multiplier for genre-based matching.</param>
        /// <param name="met">Weight multiplier for Metacritic score importance.</param>
        /// <param name="pop">Weight multiplier for game popularity.</param>
        /// <param name="howManyGames">Number of results requested from the AI model.</param>
        /// <returns>A collection of recommended games sorted by their relevance score.</returns>
        Task<PythonRecommendationResponse> GetUserRecommendationsAsync(string query, float genre, float met, float pop, int howManyGames);
    }
}
