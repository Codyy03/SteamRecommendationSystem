using steam_discovery_platform.Server.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace steam_discovery_platform.Server.Interfaces
{
    public interface IPythonRecommendationService
    {
        Task<List<GameInfoDTO>> GetRecommendationsAsync(string query, float genre, float met, float pop, int howManyGames, float series_penalty);
    }
}
