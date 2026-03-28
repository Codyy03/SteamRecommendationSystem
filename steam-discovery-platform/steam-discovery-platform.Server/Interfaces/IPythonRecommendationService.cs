using steam_discovery_platform.Server.DTOs;

namespace steam_discovery_platform.Server.Interfaces
{
    public interface IPythonRecommendationService
    {
        Task<List<GameInfoDTO>> GetRecommendationsAsync(string query);
    }
}
